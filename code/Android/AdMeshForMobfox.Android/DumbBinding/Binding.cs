using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using IOToolkit.Helpers;

namespace AdMeshForMobfox.Android.DumbBinding
{
    public interface IBindable
    {
        object ProperyInstance { get; set; }
    }

    public abstract class BindableBase<TInstance> : IBindable where TInstance : class
    {
        public TInstance ContainerInstance { get; set; }
        public abstract object ProperyInstance { get; set; }


    }

    public class Bindable<TInstance, TProperty> : BindableBase<TInstance> where TInstance : class
    {
        public Expression<Func<TInstance, TProperty>> PropertyExpression { get; set; }
        public PropertyInfo PropertyInfo { get; set; }

        public Bindable(TInstance instance, Expression<Func<TInstance, TProperty>> exp)
            : this(exp)
        {
            SetupBindable(instance);
        }

        public Bindable(Expression<Func<TInstance, TProperty>> exp)
        {
            PropertyExpression = exp;
        }

        public override object ProperyInstance
        {
            get
            {
                return ContainerInstance == null ? null : PropertyInfo.GetValue(ContainerInstance);
            }
            set
            {
                PropertyInfo.SetValue(ContainerInstance, value);
            }
        }


        internal void SetupBindable(TInstance instance)
        {
            ContainerInstance = instance;
            if (ContainerInstance != null)
                PropertyInfo = ContainerInstance.GetPropertyInfo(PropertyExpression);
        }
    }

    public class BindableMethod<TInstance, TProperty> : BindableBase<TInstance> where TInstance : class
    {
        private readonly Action<TInstance, TProperty> _method;

        public BindableMethod(TInstance instance, Action<TInstance, TProperty> method)
        {
            _method = method;
            ContainerInstance = instance;
        }

        private object _properyInstance;

        public override object ProperyInstance
        {
            get { return _properyInstance; }
            set
            {
                _properyInstance = value;
                _method(ContainerInstance, (TProperty)_properyInstance);
            }


        }
    }

    public class BindableTarget
    {
        public IBindable Target { get; set; }
        public Func<object, object> Converter { get; set; }
    }


    public interface IBinder
    {
        void OnContainerInstanceChanged(object newInstance);
    }


    public class Binder<TInstance, TProperty> : IBinder
        where TInstance : class, INotifyPropertyChanged
    {
        private readonly Bindable<TInstance, TProperty> _source;
        private readonly ICollection<IBinder> _childBinds = new List<IBinder>();
        private readonly ICollection<BindableTarget> _binds = new List<BindableTarget>();

        public Binder(TInstance instance, Expression<Func<TInstance, TProperty>> exp)
        {
            _source = new Bindable<TInstance, TProperty>(exp);
            OnContainerInstanceChanged(instance);
        }

        public void OnContainerInstanceChanged(object newInstance)
        {

            if (newInstance == _source.ContainerInstance)
                return;

            //
            //  Cleanup previous events
            //  Notify children to do the same
            //
            if (_source.ContainerInstance != null)
            {
                _source.ContainerInstance.PropertyChanged -= ContainerInstance_PropertyChanged;
                foreach (var childBind in _childBinds)
                {
                    childBind.OnContainerInstanceChanged(null);
                }
            }

            _source.SetupBindable((TInstance)newInstance);

            if (_source.ContainerInstance == null)
                return;
            _source.ContainerInstance.PropertyChanged += ContainerInstance_PropertyChanged;


            OnPropertyChanged();
        }


        void ContainerInstance_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == _source.PropertyInfo.Name)
            {
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged()
        {
            var propInstance = _source.ProperyInstance;

            foreach (var childBind in _childBinds)
            {
                childBind.OnContainerInstanceChanged(propInstance);
            }

            foreach (var bindableTarget in _binds)
            {
                SetNewValueOnBindedProp(propInstance, bindableTarget);
            }
        }

        private static void SetNewValueOnBindedProp(object propInstance, BindableTarget bindableTarget)
        {
            if (bindableTarget.Converter != null && propInstance != null)
                propInstance = bindableTarget.Converter(propInstance);

            bindableTarget.Target.ProperyInstance = propInstance;
        }

        protected void AddChildBinding(IBinder binder)
        {
            _childBinds.Add(binder);
            binder.OnContainerInstanceChanged(_source.ProperyInstance);
        }

        public Binder<TInstance, TProperty> BindTo<TTarget, TTargetProp>(TTarget target, Expression<Func<TTarget, TTargetProp>> targetProp, Func<TProperty, TTargetProp> converter)
            where TTarget : class
        {
            var bindTarget = new BindableTarget
            {
                Target = new Bindable<TTarget, TTargetProp>(target, targetProp),
            };

            if (converter != null)
            {
                bindTarget.Converter = (obj) => converter((TProperty)obj);
            }

            _binds.Add(bindTarget);


            SetNewValueOnBindedProp(_source.ProperyInstance, bindTarget);

            return this;
        }

        public Binder<TInstance, TProperty> BindToMethod<TTarget>(TTarget target, Action<TTarget, TProperty> method)
            where TTarget : class
        {
            var bindTarget = new BindableTarget
            {
                Target = new BindableMethod<TTarget, TProperty>(target, method)
            };

            _binds.Add(bindTarget);


            SetNewValueOnBindedProp(_source.ProperyInstance, bindTarget);

            return this;
        }

        public Binder<TInstance, TProperty> BindTo<TTarget>(TTarget target, Expression<Func<TTarget, TProperty>> targetProp)
          where TTarget : class
        {
            return BindTo(target, targetProp, null);
        }

    }

    public class NotifyPropBinder<TInstance, TProperty> : Binder<TInstance, TProperty>
        where TInstance : class, INotifyPropertyChanged
        where TProperty : class, INotifyPropertyChanged
    {
        public NotifyPropBinder(TInstance instance, Expression<Func<TInstance, TProperty>> exp)
            : base(instance, exp)
        {
        }

        public NotifyPropBinder(Expression<Func<TInstance, TProperty>> exp)
            : base(null, exp)
        {
        }

        public NotifyPropBinder<TProperty, TNewInstance> ChainBind<TNewInstance>(Expression<Func<TProperty, TNewInstance>> exp)
        where TNewInstance : class, INotifyPropertyChanged
        {
            var ret = new NotifyPropBinder<TProperty, TNewInstance>(exp);
            AddChildBinding(ret);
            return ret;
        }

        public Binder<TProperty, TNewInstance> CreateBinding<TNewInstance>(Expression<Func<TProperty, TNewInstance>> exp)
        {
            var ret = new Binder<TProperty, TNewInstance>(null, exp);
            AddChildBinding(ret);
            return ret;
        }
    }

    public static class BinderExtensions
    {
        public static Binder<TInstance, TProperty> CreateBinding<TInstance, TProperty>(this TInstance instance,
            Expression<Func<TInstance, TProperty>> prop)
            where TInstance : class, INotifyPropertyChanged
        {
            return new Binder<TInstance, TProperty>(instance, prop);
        }

        public static NotifyPropBinder<TInstance, TProperty> CreateBindingChain<TInstance, TProperty>(this TInstance instance,
          Expression<Func<TInstance, TProperty>> prop)
            where TInstance : class, INotifyPropertyChanged
            where TProperty : class, INotifyPropertyChanged
        {
            return new NotifyPropBinder<TInstance, TProperty>(instance, prop);
        }

        public static Binder<TInstance, TProperty> BindTo<TInstance, TProperty, TTarget>(this Binder<TInstance, TProperty> binder, TTarget target, Expression<Func<TTarget, string>> textProperty)
            where TInstance : class, INotifyPropertyChanged
            where TTarget : class
        {
            return binder.BindTo(target, textProperty, p => p.ToString());
        }
    }

    public static class AndroidBinderExtensions
    {
        public static Binder<TInstance, TProperty> BindTo<TInstance, TProperty>(
            this Binder<TInstance, TProperty> binder, TextView target)
            where TInstance : class, INotifyPropertyChanged
        {
            return binder.BindTo(target, t => t.Text);
        }

        public static Binder<TInstance, TProperty> BindTo<TInstance, TProperty>(
           this Binder<TInstance, TProperty> binder, TextView target, Func<TProperty, string> converter)
           where TInstance : class, INotifyPropertyChanged
        {
            return binder.BindTo(target, t => t.Text, converter);
        }

        public static Binder<TInstance, TProperty> BindTo<TInstance, TProperty>(
          this Binder<TInstance, TProperty> binder, TextView target, Func<TProperty, object> converter)
          where TInstance : class, INotifyPropertyChanged
        {
            return binder.BindTo(target, t => t.Text, p => converter(p).ToString());
        }


        public static Binder<TInstance, TProperty> BindTo<TInstance, TProperty>(
            this Binder<TInstance, TProperty> binder, ImageView target, Action<ImageView, TProperty> method)
            where TInstance : class, INotifyPropertyChanged
        {
            return binder.BindToMethod(target, method);
        }

        public static Binder<TInstance, TProperty> BindTo<TInstance, TProperty>(
            this Binder<TInstance, TProperty> binder, ImageView target, Func<TProperty, int> getDrawableId)
            where TInstance : class, INotifyPropertyChanged
        {
            return binder.BindToMethod(target, (t, p) => t.SetImageResource(getDrawableId(p)));
        }

    }

}