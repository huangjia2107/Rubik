using System;
using System.Linq;
using System.Windows;
using System.Collections.Generic;

using Rubik.Theme.Datas;

namespace Rubik.Theme.UI
{
    public class TreeFilterUtil<T> : IDisposable where T : TreeViewModelBase
    {
        private IEnumerable<T> _source = null;
        private Stack<List<T>> _childToParentNodeStack = null;

        public TreeFilterUtil()
        {
            _childToParentNodeStack = new Stack<List<T>>();
        }

        public void Configure(IEnumerable<T> source)
        {
            _source = source;

            _childToParentNodeStack.Clear();
            _childToParentNodeStack.TrimExcess();

            ConstructC2PStack(_source, new List<T>());
        }

        /// <summary>
        /// 若当前节点可见：
        /// 1.该节点以上的父节点均得可见
        /// 2.若该节点所有子节点均不可见，则该节点以下的子节点均得可见
        /// </summary>
        public void TryFilter(Predicate<T> filter)
        {
            if (filter == null)
            {
                TreeViewModelBase.Foreach(_source, n => n.Visibility = Visibility.Visible);
                return;
            }

            foreach (var nodeList in _childToParentNodeStack)
            {
                nodeList.ForEach(node =>
                {
                    node.Visibility = filter(node) ? Visibility.Visible : Visibility.Collapsed;

                    //父节点满足条件，则所有子节点都显示
                    //父节点不满足条件，则根据是否存在显示的子节点来决定
                    if (node.Nodes != null && node.Nodes.Any())
                    {
                        if (node.Visibility == Visibility.Visible)
                            TreeViewModelBase.Foreach((IEnumerable<T>)node.Nodes, n => n.Visibility = Visibility.Visible);
                        else
                            node.Visibility = node.Nodes.Any(n => n.Visibility == Visibility.Visible) ? Visibility.Visible : Visibility.Collapsed;
                    }
                });
            }
        }

        /// <summary>
        /// 便于自下而上遍历节点
        /// </summary>
        private void ConstructC2PStack(IEnumerable<T> trees, List<T> nodes)
        {
            if (trees != null && trees.Any())
            {
                _childToParentNodeStack.Push(nodes);

                var childList = new List<T>();
                foreach (var t in trees)
                {
                    nodes.Add(t);
                    ConstructC2PStack((IEnumerable<T>)t.Nodes, childList);
                }
            }
        }

        public void Dispose()
        {
            _source = null;

            _childToParentNodeStack.Clear();
            _childToParentNodeStack.TrimExcess();
        }
    }
}
