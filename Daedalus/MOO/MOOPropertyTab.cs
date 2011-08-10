using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms.Design;

namespace Daedalus.MOO
{
    class MOOPropertyTab : PropertyTab
    {
        public override PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes)
        {
            return new PropertyDescriptorCollection((component as MOOObject).Properties.Select(p => new MOOPropertyDescriptor(component as MOOObject,p)).ToArray());
        }

        public override string TabName
        {
            get { return "Props"; }
        }

        public override bool CanExtend(object extendee)
        {
            return extendee is MOOObject;
        }
        public override System.Drawing.Bitmap Bitmap
        {
            get
            {
                return null;
                //return Properties.Resources.properties.ToBitmap();
            }
        }
    }

    public class MOOPropertyDescriptor : PropertyDescriptor
    {
        MOOObject obj;
        string prop;
        public MOOPropertyDescriptor(MOOObject obj, string prop) : base(prop, new Attribute[] {})
        {
            this.obj = obj;
            this.prop = prop;
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override Type ComponentType
        {
            get 
            {
                return obj.GetType();
            }
        }

        public override object GetValue(object component)
        {
            return this.obj[prop]; 
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override Type PropertyType
        {
            get
            {
                object value = this.obj[prop];
                if (value == null)
                    return typeof(object);
                return value.GetType();
            }
        }

        public override void ResetValue(object component)
        {
            throw new NotImplementedException();
        }

        public override void SetValue(object component, object value)
        {
            obj.SetProp(this.prop, value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }
}
