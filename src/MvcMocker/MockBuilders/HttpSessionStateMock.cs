using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace MvcMocker.MockBuilders
{
    public class HttpSessionStateMock : IMockBuilder
    {
        private IDictionary<String, Object> values;
        private HttpCookieMode mode;

        public HttpSessionStateMock()
        {
            this.values = new Dictionary<String, Object>();
        }

        public HttpSessionStateMock Add(String key, Object value)
        {
            if (String.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            this.values.Add(key, value);
            return this;
        }

        public HttpSessionStateMock Add(IDictionary<String, Object> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            this.values = values;
            return this;
        }

        public HttpSessionStateMock SetMode(HttpCookieMode mode)
        {
            this.mode = mode;
            return this;
        }

        void IMockBuilder.BuildIn(Mock<HttpContextBase> contextMock)
        {
            var dic = new Dictionary<String, Object>(values);

            contextMock.SetupGet(c => c.Session)
                .Returns(new HttpSessionDictionary(dic, mode));
        }

        private sealed class HttpSessionDictionary : HttpSessionStateBase
        {
            private readonly IDictionary<String, Object> values;
            private HttpCookieMode mode;

            public HttpSessionDictionary(IDictionary<String, Object> values, HttpCookieMode mode)
            {
                if (values == null)
                    throw new ArgumentNullException("values");

                this.values = values;
                this.mode = mode;
            }

            public override NameObjectCollectionBase.KeysCollection Keys
            {
                get { return GetAllKeys(); }
            }

            public override HttpCookieMode CookieMode
            {
                get { return mode; }
            }

            public override object this[String name]
            {
                get { return (values.ContainsKey(name)) ? values[name] : null; }
                set { values[name] = value; }
            }

            public override object this[Int32 index]
            {
                get { return GetByIndex(index); }
                set { SetByIndex(index, value); }
            }

            public override void Remove(String name)
            {
                if (String.IsNullOrEmpty(name))
                    throw new ArgumentNullException("name");

                values.Remove(name);
            }

            public override void RemoveAll()
            {
                values.Clear();
            }

            public override void RemoveAt(Int32 index)
            {
                if (IsOutOfRange(index))
                    throw new IndexOutOfRangeException("index");

                var item = values.ElementAt(index);
                values.Remove(item);
            }

            public override Int32 Count
            {
                get { return values.Count; }
            }

            public override void Add(String name, Object value)
            {
                if (String.IsNullOrEmpty(name))
                    throw new ArgumentNullException("name");

                this.values.Add(name, value);
            }

            public override void Clear()
            {
                values.Clear();
            }

            public override IEnumerator GetEnumerator()
            {
                return values.GetEnumerator();
            }

            private Object GetByIndex(Int32 index)
            {
                if (IsOutOfRange(index))
                    throw new IndexOutOfRangeException("index");

                return values.ElementAt(index).Value;
            }

            private void SetByIndex(Int32 index, Object value)
            {
                if (IsOutOfRange(index))
                    throw new IndexOutOfRangeException("index");

                KeyValuePair<String, Object> item;

                var cloned = new Dictionary<String, Object>();

                var items = values.GetEnumerator();
                for (var i = 0; i < index; i++)
                {
                    if (!items.MoveNext())
                        break;

                    item = items.Current;
                    cloned.Add(item.Key, item.Value);
                }

                item = values.ElementAt(index);
                cloned.Add(item.Key, value);

                for (var i = index+1; i < values.Count; i++)
                {
                    if (!items.MoveNext())
                        break;

                    item = items.Current;
                    cloned.Add(item.Key, item.Value);
                }
            }

            private NameObjectCollectionBase.KeysCollection GetAllKeys()
            {
                var collection = new NameValueCollection();

                foreach (var value in values)
                    collection.Add(value.Key, null);

                return collection.Keys;
            }

            private Boolean IsOutOfRange(Int32 index)
            {
                return index < 0 || index > values.Count - 1;
            }
        }
    }
}