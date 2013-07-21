using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sarcasm.DomainCore
{
    public interface IdentityWithGuid
    {
        Guid Guid { get; }
    }

    public class Name
    {
        public string Value { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class NameRef
    {
        public NameRef(string value)
        {
            this.Value = value;
        }

        public string Value { get; private set; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public interface Reference
    {
        NameRef NameRef { get; set; }
        object Target { get; set; }
    }

    public interface Reference<out T> : Reference
    {
        new T Target { get; }
    }

    internal interface ICopyableReference
    {
        Reference CopyWithoutReference();
    }

    internal class ReferenceImpl<T> : Reference<T>, ICopyableReference
    {
        public NameRef NameRef { get; set; }
        public Guid? GuidRef { get; set; }
        public T Target { get; set; }
        object Reference.Target { get { return Target; } set { Target = (T)value; } }

        public ReferenceImpl(NameRef nameRef)
        {
            this.NameRef = nameRef;
        }

        public ReferenceImpl(Guid guidRef)
        {
            this.GuidRef = guidRef;
        }

        public ReferenceImpl(T target)
        {
            this.Target = target;

            if (Target != null && Target is IdentityWithGuid)
                this.GuidRef = ((IdentityWithGuid)Target).Guid;
        }

        public override string ToString()
        {
            if (NameRef != null)
                return string.Format("[refByName: {0}]", NameRef.ToString());
            else if (GuidRef != null)
                return string.Format("[refByGuid: {0}]", GuidRef.ToString());
            else
                return string.Format("[refByTarget: {0}]", Target.ToString());
        }

        Reference ICopyableReference.CopyWithoutReference()
        {
            return new ReferenceImpl<T>(NameRef) { GuidRef = GuidRef };
        }
    }

    public static class ReferenceFactory
    {
        public static Reference<T> Get<T>(T target)
        {
            return new ReferenceImpl<T>(target);
        }

        public static Reference<T> Get<T>(NameRef nameRef)
        {
            return new ReferenceImpl<T>(nameRef);
        }

        public static Reference<T> Get<T>(Guid guidRef)
        {
            return new ReferenceImpl<T>(guidRef);
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class OptionalAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class NonEmptyListAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class InfoAttribute : Attribute
    {
        public string Text { get; private set; }

        public InfoAttribute(string text)
        {
            this.Text = text;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class TypeOfAttribute : Attribute
    {
        public Type[] Types { get; private set; }

        public TypeOfAttribute(params Type[] types)
        {
            this.Types = types;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DomainRootAttribute : Attribute
    {
        public string Name { get; private set; }

        public DomainRootAttribute(string name)
        {
            this.Name = name;
        }
    }
}
