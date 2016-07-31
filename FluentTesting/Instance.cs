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

        public Instance GetResults()
        {
            CheckResults();
            return this;
        }

        public object GetPropertyValue(string propertyName)
        {
            return Target.GetType().GetProperty(propertyName).GetValue(Target);
        }

        protected void CheckResults()
        {
            ResultsPassed = true;
            var log = new StringBuilder(Description);

            foreach(var propertyName in Properties.Keys)
            {
                var value = Target.GetType().GetProperty(propertyName).GetValue(Target);

                foreach(var item in Properties[propertyName])
                {
                    log.AppendFormat(" {0} {1} = [{2}] {3} [{4}]", 
                        item.Condition, propertyName, value, item.Operator, item.Value);
                    switch(item.Condition)
                    {
                        case eCondition.And:
                            switch(item.Operator)
                            {
                                case eOperator.isEqualTo:
                                    ResultsPassed = ResultsPassed && value._IsEqualTo(item.Value);
                                    break;
                                case eOperator.isNotEqualTo:
                                    ResultsPassed = ResultsPassed && value._IsNotEqualTo(item.Value);
                                    break;
                                case eOperator.isGreaterThan:
                                    ResultsPassed = ResultsPassed && value._IsGreaterThan(item.Value);
                                    break;
                                case eOperator.isLessThan:
                                    ResultsPassed = ResultsPassed && value._IsLessThan(item.Value);
                                    break;
                                case eOperator.isGreaterThanOrEqual:
                                    ResultsPassed = ResultsPassed && value._IsGreaterThanOrEqual(item.Value);
                                    break;
                                case eOperator.isLessThanOrEqual:
                                    ResultsPassed = ResultsPassed && value._IsLessThanOrEqual(item.Value);
                                    break;
                            }
                            break;
                        case eCondition.Or:
                            switch (item.Operator)
                            {
                                case eOperator.isEqualTo:
                                    ResultsPassed = ResultsPassed || value._IsEqualTo(item.Value);
                                    break;
                                case eOperator.isNotEqualTo:
                                    ResultsPassed = ResultsPassed || value._IsNotEqualTo(item.Value);
                                    break;
                                case eOperator.isGreaterThan:
                                    ResultsPassed = ResultsPassed || value._IsGreaterThan(item.Value);
                                    break;
                                case eOperator.isLessThan:
                                    ResultsPassed = ResultsPassed || value._IsLessThan(item.Value);
                                    break;
                                case eOperator.isGreaterThanOrEqual:
                                    ResultsPassed = ResultsPassed || value._IsGreaterThanOrEqual(item.Value);
                                    break;
                                case eOperator.isLessThanOrEqual:
                                    ResultsPassed = ResultsPassed || value._IsLessThanOrEqual(item.Value);
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

            log.AppendFormat(" : {0}", ResultsPassed);
            Common.Extensions.TraceLog.Information("{log}", log);
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
                return new Item(Parent, value, condition, eOperator.isEqualTo);
            }

            public Item IsNotEqualTo(object value, eCondition condition = eCondition.And)
            {
                return new Item(Parent, value, condition, eOperator.isNotEqualTo);
            }
            public Item IsGreaterThan(object value, eCondition condition = eCondition.And)
            {
                return new Item(Parent, value, condition, eOperator.isGreaterThan);
            }
            public Item IsLessThan(object value, eCondition condition = eCondition.And)
            {
                return new Item(Parent, value, condition, eOperator.isLessThan);
            }
            public Item IsGreaterThanOrEqual(object value, eCondition condition = eCondition.And)
            {
                return new Item(Parent, value, condition, eOperator.isGreaterThanOrEqual);
            }
            public Item IsLessThanOrEqual(object value, eCondition condition = eCondition.And)
            {
                return new Item(Parent, value, condition, eOperator.isLessThanOrEqual);
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
                    .IsGreaterThan(value, eCondition.Or);
            }
            public Item OrIsLessThan(object value)
            {
                return new VerifyProp(Parent, Parent.Properties.Last().Key)
                    .IsLessThan(value, eCondition.Or);
            }
            public Item OrIsGreatherThanOrEqual(object value)
            {
                return new VerifyProp(Parent, Parent.Properties.Last().Key)
                    .IsGreaterThanOrEqual(value, eCondition.Or);
            }
            public Item OrIsLessThanOrEqual(object value)
            {
                return new VerifyProp(Parent, Parent.Properties.Last().Key)
                    .IsLessThanOrEqual(value, eCondition.Or);
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
                    .IsGreaterThan(value);
            }
            public Item AndIsLessThan(object value)
            {
                return new VerifyProp(Parent, Parent.Properties.Last().Key)
                    .IsLessThan(value);
            }
            public Item AndIsGreatherThanOrEqual(object value)
            {
                return new VerifyProp(Parent, Parent.Properties.Last().Key)
                    .IsGreaterThanOrEqual(value);
            }
            public Item AndIsLessThanOrEqual(object value)
            {
                return new VerifyProp(Parent, Parent.Properties.Last().Key)
                    .IsLessThanOrEqual(value);
            }

            public Instance VerifyResults()
            {
                return Parent.GetResults();
            }
        }

        #endregion inner classes
    }

    public enum eCondition
    {
        And,
        Or
    }

    public enum eOperator
    {
        isEqualTo, //==
        isNotEqualTo, //!=
        isGreaterThan, //>
        isLessThan, //<
        isGreaterThanOrEqual, //>=
        isLessThanOrEqual //<=
    }
}
