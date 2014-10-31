﻿using System;
using System.Reflection;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace ColorGame.SL.Helpers
{
    public class CallDataContextMethodAction : TriggerAction<DependencyObject>
    {
        public string MethodName { get; set; }

        protected override void OnAttached()
        {
        }

        protected override void Invoke(object o)
        {
            FrameworkElement fwe = this.AssociatedObject as FrameworkElement;
            if (fwe == null)
                return;

            var aodc = fwe.DataContext;
            bool successful = false;
            object lastdc = null;
            while (fwe != null)
            {
                var fwedc = fwe.DataContext;
                if (fwedc == null || fwedc.Equals(lastdc))
                {
                    fwe = VisualTreeHelper.GetParent(fwe) as FrameworkElement;
                    continue;
                }

                MethodInfo mi = fwedc.GetType().GetMethod(MethodName);
                if (mi == null)
                {
                    fwe = VisualTreeHelper.GetParent(fwe) as FrameworkElement;
                }
                else
                {
                    if (mi.GetParameters().Length == 1)
                        mi.Invoke(fwedc, new object[1] { aodc });
                    else
                        mi.Invoke(fwedc, null);
                    successful = true;
                    break;
                }
            }
            if (!successful)
                throw new InvalidOperationException(String.Format("Action Invoke failed: data context does not contain a method '{0}'", MethodName));
        }
    }
}