#region License
/*
    This file is part of Sarcasm.

    Copyright 2012-2013 Dávid Németi

    Sarcasm is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Sarcasm is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Sarcasm.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using Sarcasm.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;

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

        // internal setter (instead of private) for parsers of universal grammars
        public string Value { get; internal set; }

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

    public enum NumberLiteralBase { Decimal = 10, Hexadecimal = 16, Octal = 8, Binary = 2 }

    public interface INumberLiteral
    {
        object Value { get; set; }
        NumberLiteralBase Base { get; set; }
        bool HasExplicitTypeModifier { get; set; }
    }

    public enum CommentCategory
    {
        Outline,
        Comment,
        Directive
    }

    public enum CommentKind
    {
        SingleLine,
        Delimited
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
        public CommentKind Kind { get; private set; }
        public bool IsDecorated { get; private set; }

        public Comment(string[] textLines, CommentCategory category, CommentPlacement placement, int lineIndexDistanceFromOwner, CommentKind kind, bool isDecorated)
        {
            this.TextLines = textLines;
            this.Category = category;
            this.Placement = placement;
            this.LineIndexDistanceFromOwner = lineIndexDistanceFromOwner;
            this.Kind = kind;
            this.IsDecorated = isDecorated;
        }
    }

    public class Comments
    {
        public IList<Comment> Left { get { return left; } }
        public IList<Comment> Right { get { return right; } }

        internal List<Comment> left { get; private set; }
        internal List<Comment> right { get; private set; }

        public Comments()
        {
            this.left = new List<Comment>();
            this.right = new List<Comment>();
        }
    }

    internal class ExpandoObjectLight : DynamicObject
    {
        private readonly Dictionary<string, object> propertyToValue = new Dictionary<string, object>();

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            propertyToValue[binder.Name] = value;
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            propertyToValue.TryGetValue(binder.Name, out result);
            return true;
        }
    }

    public static class ExpandoExtensions
    {
        private static readonly ConditionalWeakTable<object, ExpandoObjectLight> props = new ConditionalWeakTable<object, ExpandoObjectLight>();

        public static dynamic Props(this object key)
        {
            return props.GetOrCreateValue(key);
        }

        public static void SetComments(this object obj, Comments comments)
        {
            obj.Props().Comments = comments;
        }

        public static Comments GetComments(this object obj)
        {
            return obj.Props().Comments;
        }

        internal static void SetDirectParent(this object obj, object parent)
        {
            obj.Props().Parent = parent;
        }

        /// <summary>
        /// Get direct parent of <paramref name="obj"/>.
        /// If <paramref name="obj"/> is an item of a container, then the container object will be returned.
        /// </summary>
        public static object GetDirectParent(this object obj)
        {
            return obj.Props().Parent;
        }

        /// <summary>
        /// Get parent of <paramref name="obj"/>.
        /// If <paramref name="obj"/> is an item of a container, then object which owns the container will be returned.
        /// </summary>
        public static object GetParent(this object obj)
        {
            object directParent = obj.GetDirectParent();
            return directParent is IEnumerable ? directParent.GetDirectParent() : directParent;
        }

        /// <summary>
        /// Get the top ancestor of <paramref name="obj"/>.
        /// </summary>
        public static object GetRoot(this object obj)
        {
            return Util.RecurseStopBeforeNull(obj, objT => objT.GetDirectParent()).Last();
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
