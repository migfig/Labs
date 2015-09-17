using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace FluentTesting
{
    public class Instance
    {
        #region properties

        /// <summary>
        /// Test description
        /// </summary>
        protected string Description { get; private set; }

        /// <summary>
        /// Target object to test with
        /// </summary>
        protected object Target { get; private set; }

        /// <summary>
        /// Property name to call for testing
        /// </summary>
        protected Dictionary<string, List<PropertyItem>> Properties { get; private set; }

        public bool ResultsPassed { get; private set; }
        /// <summary>
        /// Log strategy for optional testing evidence
        /// </summary>
        protected ILogger Logger { get; private set; }

        #endregion properties

        public Instance(string description, object target)
        {
            Description = description;
            Target = target;
            Properties = new Dictionary<string, List<PropertyItem>>();
        }

        protected Instance GetResults()
        {
            CheckResults();
            return this;
        }

        protected void CheckResults()
        {
            ResultsPassed = true;
            foreach(var propertyName in Properties.Keys)
            {
                var value = Target.GetType().GetProperty(propertyName).GetValue(Target);

                foreach(var item in Properties[propertyName])
                {
                    switch(item.Condition)
                    {
                        case eCondition.And:
                            switch(item.Operator)
                            {
                                case eOperator.Equals:
                                    ResultsPassed = ResultsPassed && value.IsEqual(item.Value);
                                    break;
                                case eOperator.NotEquals:
                                    ResultsPassed = ResultsPassed && value.IsNotEqual(item.Value);
                                    break;
                                case eOperator.GreatherThan:
                                    ResultsPassed = ResultsPassed && value.IsGreaterThan(item.Value);
                                    break;
                                case eOperator.LessThan:
                                    ResultsPassed = ResultsPassed && value.IsLessThan(item.Value);
                                    break;
                                case eOperator.GreatherThanOrEquals:
                                    ResultsPassed = ResultsPassed && value.IsGreaterThanOrEqual(item.Value);
                                    break;
                                case eOperator.LessThanOrEquals:
                                    ResultsPassed = ResultsPassed && value.IsLessThanOrEqual(item.Value);
                                    break;
                            }
                            break;
                        case eCondition.Or:
                            switch (item.Operator)
                            {
                                case eOperator.Equals:
                                    ResultsPassed = ResultsPassed || value.IsEqual(item.Value);
                                    break;
                                case eOperator.NotEquals:
                                    ResultsPassed = ResultsPassed || value.IsNotEqual(item.Value);
                                    break;
                                case eOperator.GreatherThan:
                                    ResultsPassed = ResultsPassed || value.IsGreaterThan(item.Value);
                                    break;
                                case eOperator.LessThan:
                                    ResultsPassed = ResultsPassed || value.IsLessThan(item.Value);
                                    break;
                                case eOperator.GreatherThanOrEquals:
                                    ResultsPassed = ResultsPassed || value.IsGreaterThanOrEqual(item.Value);
                                    break;
                                case eOperator.LessThanOrEquals:
                                    ResultsPassed = ResultsPassed || value.IsLessThanOrEqual(item.Value);
                                    break;
                            }
                            break;
                    }

                    if (!ResultsPassed)
                        break;
                }

                if (!ResultsPassed)
                    break;
            }
        }

        public VerifyProp VerifyProperty(string properyName)
        {
            return new VerifyProp(this, properyName);
        }

        #region inner classes

        public struct PropertyItem
        {
            public eCondition Condition { get; set; }
            public eOperator Operator { get; set; }
            public object Value { get; set; }
        }

        public enum eCondition
        {
            And,
            Or
        }

        public enum eOperator
        {
            Equals, //==
            NotEquals, //!=
            GreatherThan, //>
            LessThan, //<
            GreatherThanOrEquals, //>=
            LessThanOrEquals //<=
        }

        public class VerifyProp : BaseIt<Instance>
        {
            public VerifyProp(Instance parent, string propertyName)
                : base(parent)
            {
                if(!parent.Properties.Any(x => x.Key == propertyName))
                    parent.Properties.Add(propertyName, new List<PropertyItem>());
            }

            public Item IsEqualTo(object value, eCondition condition = eCondition.And)
            {
                return new Item(Parent, value, condition, eOperator.Equals);
            }

            public Item IsNotEqualTo(object value, eCondition condition = eCondition.And)
            {
                return new Item(Parent, value, condition, eOperator.NotEquals);
            }
            public Item IsGreatherThan(object value, eCondition condition = eCondition.And)
            {
                return new Item(Parent, value, condition, eOperator.GreatherThan);
            }
            public Item _IsLessThan(object value, eCondition condition = eCondition.And)
            {
                return new Item(Parent, value, condition, eOperator.LessThan);
            }
            public Item IsGreatherThanOrEqual(object value, eCondition condition = eCondition.And)
            {
                return new Item(Parent, value, condition, eOperator.GreatherThanOrEquals);
            }
            public Item _IsLessThanOrEqual(object value, eCondition condition = eCondition.And)
            {
                return new Item(Parent, value, condition, eOperator.LessThanOrEquals);
            }
        }

        public class Item : BaseIt<Instance>
        {
            public bool Results
            {
                get
                {
                    return Parent.ResultsPassed;
                }
            }

            public Item(Instance parent, object value, eCondition condition, eOperator @operator)
                : base(parent)
            {
                var prop = parent.Properties.Last();
                var items = prop.Value;
                items.Add(new PropertyItem
                {
                    Condition = condition,
                    Operator = @operator,
                    Value = value
                });
            }

            //ors
            public Item OrIsEqualTo(object value)
            {
                return new VerifyProp(Parent, Parent.Properties.Last().Key)
                    .IsEqualTo(value, eCondition.Or);
            }
            public Item OrIsNotEqualTo(object value)
            {
                return new VerifyProp(Parent, Parent.Properties.Last().Key)
                    .IsNotEqualTo(value, eCondition.Or);
            }
            public Item OrIsGreatherThan(object value)
            {
                return new VerifyProp(Parent, Parent.Properties.Last().Key)
                    .IsGreatherThan(value, eCondition.Or);
            }
            public Item OrIsLessThan(object value)
            {
                return new VerifyProp(Parent, Parent.Properties.Last().Key)
                    ._IsLessThan(value, eCondition.Or);
            }
            public Item OrIsGreatherThanOrEqual(object value)
            {
                return new VerifyProp(Parent, Parent.Properties.Last().Key)
                    .IsGreatherThanOrEqual(value, eCondition.Or);
            }
            public Item OrIsLessThanOrEqual(object value)
            {
                return new VerifyProp(Parent, Parent.Properties.Last().Key)
                    ._IsLessThanOrEqual(value, eCondition.Or);
            }

            //ands
            public Item AndIsEqualTo(object value)
            {
                return new VerifyProp(Parent, Parent.Properties.Last().Key)
                    .IsEqualTo(value);
            }
            public Item AndIsNotEqualTo(object value)
            {
                return new VerifyProp(Parent, Parent.Properties.Last().Key)
                    .IsNotEqualTo(value);
            }
            public Item AndIsGreatherThan(object value)
            {
                return new VerifyProp(Parent, Parent.Properties.Last().Key)
                    .IsGreatherThan(value);
            }
            public Item AndIsLessThan(object value)
            {
                return new VerifyProp(Parent, Parent.Properties.Last().Key)
                    ._IsLessThan(value);
            }
            public Item AndIsGreatherThanOrEqual(object value)
            {
                return new VerifyProp(Parent, Parent.Properties.Last().Key)
                    .IsGreatherThanOrEqual(value);
            }
            public Item AndIsLessThanOrEqual(object value)
            {
                return new VerifyProp(Parent, Parent.Properties.Last().Key)
                    ._IsLessThanOrEqual(value);
            }

            public Instance VerifyResults()
            {
                return Parent.GetResults();
            }
        }

        #endregion inner classes
    }
}
