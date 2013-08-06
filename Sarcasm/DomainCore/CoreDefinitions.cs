using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sarcasm.DomainCore
{
    public enum EmptyCollectionHandling { ReturnNull, ReturnEmpty }

    public abstract class Domain
    {
        /// <summary>
        /// The default is EmptyCollectionHandling.ReturnEmpty.
        /// </summary>
        public virtual EmptyCollectionHandling EmptyCollectionHandling { get { return EmptyCollectionHandling.ReturnEmpty; } }
    }

    public abstract class Domain<TRoot> : Domain { }

    public class UniversalDefaultDomain : Domain { }

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
        // for parsers of universal grammars
        internal NameRef() { }

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
        Guid? GuidRef { get; set; }
        object Target { get; set; }
        Type Type { get; }
    }

    public interface Reference<out T> : Reference
    {
        new T Target { get; }
    }

    internal class ReferenceImpl<T> : Reference<T>
    {
        public NameRef NameRef { get; set; }
        public Guid? GuidRef { get; set; }
        public T Target { get; set; }

        object Reference.Target { get { return Target; } set { Target = (T)value; } }
        public Type Type { get { return typeof(T); } }

        // for parsers of universal grammars
        internal ReferenceImpl() { }

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
    }

    public static class ReferenceFactory
    {
        public static Reference<T> Get<T>(T target)
        {
            return new ReferenceImpl<T>(target);
        }

        public static Reference Get(object target)
        {
            return CreateInstance(target.GetType(), target);
        }

        public static Reference<T> Get<T>(NameRef nameRef)
        {
            return new ReferenceImpl<T>(nameRef);
        }

        public static Reference Get(Type type, NameRef nameRef)
        {
            return CreateInstance(type, nameRef);
        }

        public static Reference<T> Get<T>(Guid guidRef)
        {
            return new ReferenceImpl<T>(guidRef);
        }

        public static Reference Get(Type type, Guid guidRef)
        {
            return CreateInstance(type, guidRef);
        }

        private static Reference CreateInstance(Type typeArgument, params object[] args)
        {
            Type genericTypeDefinition = typeof(ReferenceImpl<>);
            Type genericType = genericTypeDefinition.MakeGenericType(typeArgument);
            return (Reference)Activator.CreateInstance(genericType, args);
        }
    }

    public enum CommentCategory
    {
        Outline,
        Comment,
        Directive
    }

    public enum CommentPlacement
    {
        OwnerLeft,
        OwnerRight
    }

    public class Comment
    {
        public string[] TextLines { get; private set; }
        public CommentCategory Category { get; private set; }
        public CommentPlacement Placement { get; private set; }
        public int LineIndexDistanceFromOwner { get; private set; }
        public bool IsMultiLine { get; private set; }

        public Comment(string[] textLines, CommentCategory category, CommentPlacement placement, int lineIndexDistanceFromOwner, bool isMultiLine)
        {
            this.TextLines = textLines;
            this.Category = category;
            this.Placement = placement;
            this.LineIndexDistanceFromOwner = lineIndexDistanceFromOwner;
            this.IsMultiLine = isMultiLine;
        }
    }

    public class Comments
    {
        public IReadOnlyList<Comment> Left { get { return left; } }
        public IReadOnlyList<Comment> Right { get { return right; } }

        internal List<Comment> left { get; private set; }
        internal List<Comment> right { get; private set; }

        public Comments()
        {
            this.left = new List<Comment>();
            this.right = new List<Comment>();
        }
    }

    public class Document
    {
        public object Root { get; private set; }
        public IReadOnlyDictionary<object, Comments> AstValueToComments { get; private set; }

        public Document(object root, IReadOnlyDictionary<object, Comments> astValueToComments)
        {
            this.Root = root;
            this.AstValueToComments = astValueToComments;
        }
    }

    public class Document<TRoot> : Document
    {
        public new TRoot Root { get { return (TRoot)base.Root; } }

        public Document(TRoot root, IReadOnlyDictionary<object, Comments> astValueToComments)
            : base(root, astValueToComments)
        {
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
}
