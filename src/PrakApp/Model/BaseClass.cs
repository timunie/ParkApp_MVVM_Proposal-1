﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace PrakApp.Model
{
    public class BaseClass : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region INotifyPropertyChanged
        // This event tells the UI to update 
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        #endregion


        #region INotifyDataErrorInfo
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => _errorsByPropertyName.Count > 0;

        private readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return null;
            }
            else
            {
                return _errorsByPropertyName.ContainsKey(propertyName)
                        ? _errorsByPropertyName[propertyName]
                        : null;
            }

        }

        public bool GetHasError(string PropertyName)
        {
            return _errorsByPropertyName.ContainsKey(PropertyName);
        }

        public void AddError(string propertyName, string error)
        {
            if (!_errorsByPropertyName.ContainsKey(propertyName))
                _errorsByPropertyName[propertyName] = new List<string>();

            if (!_errorsByPropertyName[propertyName].Contains(error))
            {
                _errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        public void ClearErrors(string propertyName)
        {
            if (_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
        public void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        public static readonly DependencyProperty WindowTitleProperty = DependencyProperty.RegisterAttached("WindowTitleProperty",
                typeof(string), typeof(UserControl),
                new FrameworkPropertyMetadata(null, WindowTitlePropertyChanged));

        public static string GetWindowTitle(DependencyObject element)
        {
            return (string)element.GetValue(WindowTitleProperty);
        }

        public static void SetWindowTitle(DependencyObject element, string value)
        {
            element.SetValue(WindowTitleProperty, value);
        }

        private static void WindowTitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Application.Current.MainWindow.Title = e.NewValue.ToString();
        }

        #endregion
    }
}
