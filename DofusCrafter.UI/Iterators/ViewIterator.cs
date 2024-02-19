using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DofusCrafter.UI.Iterators
{
    public class ViewIterator : IEnumerator<ContentControl>
    {
        private ContentControl[]? _controls;

        object IEnumerator.Current => Current;

        private int _position = -1;

        public int Position
        {
            get => _position;
        }

        public ContentControl Current
        {
            get
            {
                if (_controls is null)
                {
                    throw new NullReferenceException(nameof(_controls));
                }

                if (_position < 0 || _position >= _controls.Length)
                {
                    throw new IndexOutOfRangeException(nameof(_position));
                }

                return _controls[_position];
            }
        }
        public int Length
        {
            get => _controls is not null ? _controls.Length : throw new NullReferenceException(nameof(_controls));
        }

        public ViewIterator()
        {
            _position = 0;
            _controls = new ContentControl[0];
        }

        public ContentControl? this[string name]
        {
            get
            {
                if (_controls is null)
                {
                    throw new NullReferenceException($"{nameof(_controls)} is null");
                }

                for (int i = 0; i < _controls.Length; i++)
                {
                    if (_controls[i].Name == name)
                    {
                        return _controls[i];
                    }
                }

                return null;
            }
        }

        public ContentControl this[int index]
        {
            get
            {
                if (_controls is null)
                {
                    throw new NullReferenceException($"{nameof(_controls)} is null");
                }

                if (index < 0 || index >= _controls.Length)
                {
                    throw new IndexOutOfRangeException($"{nameof(index)} range is out of controls length");
                }

                return _controls[index];
            }
        }

        public void Add(ContentControl contentControl)
        {
            if (contentControl is null)
            {
                throw new ArgumentNullException(nameof(contentControl));
            }

            if (_controls is null)
            {
                throw new NullReferenceException(nameof(_controls));
            }

            ContentControl[] controls = new ContentControl[_controls.Length + 1];

            if (_controls.Length > 0)
            {
                Array.Copy(_controls, controls, _controls.Length);
            }

            controls[^1] = contentControl;

            _controls = controls;
        }

        public bool Any(Func<ContentControl, bool> predicate)
        {
            if (_controls is null)
            {
                throw new NullReferenceException(nameof(_controls));
            }

            for (int i = 0; i < _controls.Length; i++)
            {
                if (predicate.Invoke(_controls[i]))
                {
                    return true;
                }
            }

            return false;
        }

        public void Dispose()
        {
            _controls = null;
            _position = -1;
        }

        public bool MoveNext()
        {
            if (_controls is null)
            {
                throw new NullReferenceException(nameof(_controls));
            }

            if (_position + 1 < _controls.Length)
            {
                _position++;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Move to the previous element in the iterator and remove the current element
        /// </summary>
        /// <returns>true If moving to the previous element is possible. false Otherwise</returns>
        /// <exception cref="NullReferenceException"></exception>
        public bool MovePrevious(bool removeCurrent = false)
        {
            if (_controls is null) throw new NullReferenceException(nameof(_controls));
            if (_position - 1 >= 0)
            {
                _position--;

                if(removeCurrent)
                {
                    ContentControl[] temp = new ContentControl[_controls.Length - 1];

                    Array.Copy(_controls, temp, _controls.Length - 1);

                    _controls = temp;
                }

                return true;
            }
            return false;
        }

        public void Reset()
        {
            _controls = new ContentControl[0];
            _position = -1;
        }
    }
}
