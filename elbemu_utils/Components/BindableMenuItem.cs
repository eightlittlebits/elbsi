using System;
using System.Windows.Forms;

namespace elbemu_utils.Components
{
    // https://social.msdn.microsoft.com/Forums/windows/en-US/0b8cba1e-f7ce-4ab0-a45b-2093dc38afc8/bind-property-in-toolstripmenuitem
    public class BindableMenuItem : MenuItem, IBindableComponent
    {
        private BindingContext _bindingContext;
        private ControlBindingsCollection _dataBindings;

        public BindableMenuItem()
        {
        }

        public BindableMenuItem(string text)
            : base(text)
        {
        }

        public BindableMenuItem(string text, EventHandler onClick)
            : base(text, onClick)
        {
        }

        public BindableMenuItem(string text, MenuItem[] items)
            : base(text, items)
        {
        }

        public BindableMenuItem(string text, EventHandler onClick, Shortcut shortcut)
            : base(text, onClick, shortcut)
        {
        }

        public BindableMenuItem(MenuMerge mergeType, int mergeOrder, Shortcut shortcut, string text, EventHandler onClick, EventHandler onPopup, EventHandler onSelect, MenuItem[] items)
            : base(mergeType, mergeOrder, shortcut, text, onClick, onPopup, onSelect, items)
        {
        }

        public ControlBindingsCollection DataBindings => _dataBindings ?? (_dataBindings = new ControlBindingsCollection(this));

        public BindingContext BindingContext { get => _bindingContext ?? (_bindingContext = new BindingContext()); set => _bindingContext = value; }
    }

}
