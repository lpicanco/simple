﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace BasicLibrary.Reflection
{
    public class EntityHelper
    {
        protected List<string> _ids = new List<string>();
        protected Type _entityType = null;
        protected object _obj = null;

        public EntityHelper(Type entityType)
        {
            _entityType = entityType;
        }

        public EntityHelper(object obj)
        {
            if (obj == null)
                throw new InvalidOperationException("Cannot use null valued object in EntityHelper constructor");
            _entityType = obj.GetType();
            _obj = obj;
        }

        public void AddID(params string[] propName)
        {
            _ids.AddRange(propName);
        }

        public bool ObjectEquals(object obj1, object obj2)
        {
            if (obj1 == null ^ obj2 == null) return false;
            if (obj1 == null && obj2 == null) return true;
            if (!_entityType.IsAssignableFrom(obj1.GetType())) return false;
            if (!_entityType.IsAssignableFrom(obj2.GetType())) return false;

            foreach (string idProp in _ids)
            {
                PropertyInfo info = _entityType.GetProperty(idProp);
                object value1 = info.GetValue(obj1, null);
                object value2 = info.GetValue(obj2, null);

                if (value1 == null ^ value2 == null) return false;
                if (value1 != null && value2 != null && !value1.Equals(value2)) return false;
            }

            return true;
        }

        public bool ObjectEquals(object obj2)
        {
            if (_obj == null)
                throw new InvalidOperationException("Intern object reference not intialized");

            return ObjectEquals(_obj, obj2);
        }
    }
}
