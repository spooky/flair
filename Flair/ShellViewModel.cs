using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Flair.Transformations;

// TODO: remember last choice

namespace Flair
{
    public class ShellViewModel : PropertyChangedBase
    {
        private bool isBusy;
        public bool IsBusy 
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                NotifyOfPropertyChange(() => IsBusy);
            }
        }

        private string source;
        public string Source
        {
            get { return source; }
            set
            {
                source = value;
                NotifyOfPropertyChange(() => Source);
                Notify(SourceChanged);
            }
        }

        private string target;
        public string Target
        {
            get { return target; }
            set
            {
                target = value;
                NotifyOfPropertyChange(() => Target);
            }
        }

        private ITransform activeTransform;
        public ITransform ActiveTransform
        {
            get { return activeTransform; }
            set
            {
                activeTransform = value;
                NotifyOfPropertyChange(() => ActiveTransform);
                Notify(TransformChanged);
            }
        }

        public IList<ITransform> Transformations { get; private set; }

        public event EventHandler TransformChanged;

        public event EventHandler SourceChanged;

        public ShellViewModel()
        {
            Transformations = IoC.GetAllInstances(typeof(ITransform))
                .Where(t => t.GetType() != typeof(CopyCat))
                .Cast<ITransform>()
                .OrderBy(t => t.DisplayText)
                .ToList();
            Transformer.Instance.Subscribe(this);

            RestoreSelectedItem();
        }

        private void RestoreSelectedItem()
        {
            if (ActiveTransform == null)
                ActiveTransform = Transformations.FirstOrDefault();
        }

        private void Notify(EventHandler eventToRaise)
        {
            if (eventToRaise != null)
                eventToRaise(this, EventArgs.Empty);
        }
    }
}