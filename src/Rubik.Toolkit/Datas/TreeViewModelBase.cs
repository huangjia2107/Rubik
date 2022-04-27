﻿using System;
using System.Windows;
using System.Collections.Generic;

using Rubik.Toolkit.Mvvm;

namespace Rubik.Toolkit.Datas
{
    public abstract class TreeViewModelBase : BindableBase
    {
        protected TreeViewModelBase() { }

        private Visibility _visibility = Visibility.Visible;
        public Visibility Visibility
        {
            get { return _visibility; }
            set { SetProperty(ref _visibility, value); }
        }

        public virtual IEnumerable<TreeViewModelBase> Nodes { get; set; }

        public bool Any<T>(Predicate<T> predicate, bool checkSelf = false) where T : TreeViewModelBase
        {
            if (checkSelf)
            {
                if (predicate((T)this))
                    return true;
            }

            return Any((IEnumerable<T>)Nodes, predicate);
        }

        public bool All<T>(Predicate<T> predicate, bool checkSelf = false) where T : TreeViewModelBase
        {
            if (checkSelf)
            {
                if (!predicate((T)this))
                    return false;
            }

            foreach (T node in Nodes)
            {
                if (!node.All(predicate, true))
                    return false;
            }

            return true;
        }

        public static bool Any<T>(IEnumerable<T> collection, Predicate<T> predicate) where T : TreeViewModelBase
        {
            if (collection == null || predicate == null)
                return false;

            foreach (var n in collection)
            {
                if (predicate(n))
                    return true;

                if (Any((IEnumerable<T>)n.Nodes, predicate))
                    return true;
            }

            return false;
        }

        public static T Find<T>(IEnumerable<T> collection, Predicate<T> predicate) where T : TreeViewModelBase
        {
            if (collection == null || predicate == null)
                return null;

            foreach (var n in collection)
            {
                if (predicate(n))
                    return n;

                var result = Find((IEnumerable<T>)n.Nodes, predicate);
                if (result != null)
                    return result;
            }

            return null;
        }

        public static int IndexOf<T>(IEnumerable<T> collection, Predicate<T> predicate) where T : TreeViewModelBase
        {
            var index = -1;

            if (collection == null || predicate == null)
                return index;

            foreach (var n in collection)
            {
                index++;

                if (predicate(n))
                    return index;
            }

            return -1;
        }

        public static bool Update<T>(IEnumerable<T> collection, Predicate<T> predicate) where T : TreeViewModelBase
        {
            if (collection == null || predicate == null)
                return false;

            foreach (var n in collection)
            {
                if (predicate(n))
                    return true;

                var result = Update((IEnumerable<T>)n.Nodes, predicate);
                if (result)
                    return true;
            }

            return false;
        }

        public static void Update<T>(IEnumerable<T> collection, Action<T> update) where T : TreeViewModelBase
        {
            if (collection == null || update == null)
                return;

            foreach (var n in collection)
            {
                update.Invoke(n);
                Update((IEnumerable<T>)n.Nodes, update);
            }
        }
    }
}
