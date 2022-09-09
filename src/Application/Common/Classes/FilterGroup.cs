using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ZenAchitecture.Application.Common.Classes
{
    public class FilterGroup
    {
        public string GroupKey { get; set; }
        public string GroupName { get; set; }
        public List<Property> Properties { get; set; }
        public int Position { get; set; }

        public static List<FilterGroup> CreateGroups(IStringLocalizer stringLocalizer)
        {
            return new List<FilterGroup>()
                {
                    new FilterGroup()
                    {
                        GroupKey = "mostUsed",
                        GroupName = stringLocalizer.GetString("AdvanceFilter.MostUsed"),
                        Position = 1,
                        Properties = new List<Property>()
                        {
                            new Property()
                            {
                                Key = FilterPropertyKeyConstants.JOBS,
                                Name = stringLocalizer.GetString("AdvanceFilter.CurrentJobTitle"),
                                Operators = Operator.CreateListByPropertyType(PropertyType.LIST, stringLocalizer),
                                Type = PropertyType.LIST,
                                FilterType = FilterType.RELATION
                            },
                            new Property()
                            {
                                Key = FilterPropertyKeyConstants.TAGS,
                                Name = stringLocalizer.GetString("AdvanceFilter.Tags"),
                                Operators = Operator.CreateListByPropertyType(PropertyType.LIST, stringLocalizer),
                                Type = PropertyType.LIST,
                                FilterType = FilterType.RELATION
                            },
                            new Property()
                            {
                                Key = FilterPropertyKeyConstants.ADDRESS,
                                Name = stringLocalizer.GetString("AdvanceFilter.Address"),
                                Operators = Operator.CreateListByPropertyType(PropertyType.STRING, stringLocalizer),
                                Type = PropertyType.STRING,
                                FilterType = FilterType.COLUMN
                            }
                        }
                    },
                    new FilterGroup()
                    {
                        GroupKey = "candidate",
                        GroupName = stringLocalizer.GetString("AdvanceFilter.Candidate"),
                        Position = 2,
                        Properties = new List<Property>()
                        {
                            new Property()
                            {
                                Key = FilterPropertyKeyConstants.NAME,
                                Name = stringLocalizer.GetString("AdvanceFilter.Name"),
                                Operators = Operator.CreateListByPropertyType(PropertyType.STRING, stringLocalizer),
                                Type = PropertyType.STRING,
                                FilterType = FilterType.COLUMN
                            },
                            new Property()
                            {
                                Key = FilterPropertyKeyConstants.CURRENT_JOB_TITLE,
                                Name = stringLocalizer.GetString("AdvanceFilter.CurrentJobTitle"),
                                Operators = Operator.CreateListByPropertyType(PropertyType.STRING, stringLocalizer),
                                Type = PropertyType.STRING,
                                FilterType = FilterType.COLUMN
                            },
                            new Property()
                            {
                                Key = FilterPropertyKeyConstants.SKILLS,
                                Name = stringLocalizer.GetString("AdvanceFilter.Skills"),
                                Operators = Operator.CreateListByPropertyType(PropertyType.LIST, stringLocalizer),
                                Type = PropertyType.LIST,
                                FilterType = FilterType.RELATION
                            },
                            new Property()
                            {
                                Key = FilterPropertyKeyConstants.LANGUAGES,
                                Name = stringLocalizer.GetString("AdvanceFilter.Languages"),
                                Operators = Operator.CreateListByPropertyType(PropertyType.LIST, stringLocalizer),
                                Type = PropertyType.LIST,
                                FilterType = FilterType.RELATION
                            },
                            new Property()
                            {
                                Key = FilterPropertyKeyConstants.EMAILS,
                                Name = stringLocalizer.GetString("AdvanceFilter.Emails"),
                                Operators = Operator.CreateListByPropertyType(PropertyType.STRING, stringLocalizer),
                                Type = PropertyType.STRING,
                                FilterType = FilterType.COLUMN
                            },
                            new Property()
                            {
                                Key = FilterPropertyKeyConstants.PHONES,
                                Name = stringLocalizer.GetString("AdvanceFilter.Phones"),
                                Operators = Operator.CreateListByPropertyType(PropertyType.STRING, stringLocalizer),
                                Type = PropertyType.STRING,
                                FilterType = FilterType.COLUMN
                            },
                            new Property()
                            {
                                Key = FilterPropertyKeyConstants.EDUCATIONS,
                                Name = stringLocalizer.GetString("AdvanceFilter.Educations"),
                                Operators = Operator.CreateListByPropertyType(PropertyType.STRING, stringLocalizer),
                                Type = PropertyType.STRING,
                                FilterType = FilterType.COLUMN
                            },
                            new Property()
                            {
                                Key = FilterPropertyKeyConstants.EXPERIENCES,
                                Name = stringLocalizer.GetString("AdvanceFilter.Experiences"),
                                Operators = Operator.CreateListByPropertyType(PropertyType.STRING, stringLocalizer),
                                Type = PropertyType.STRING,
                                FilterType = FilterType.COLUMN
                            },
                            new Property()
                            {
                                Key = FilterPropertyKeyConstants.AVATAAR,
                                Name = stringLocalizer.GetString("AdvanceFilter.Avatar"),
                                Operators = Operator.CreateListByPropertyType(PropertyType.BOOLEAN, stringLocalizer),
                                Type = PropertyType.BOOLEAN,
                                FilterType = FilterType.COLUMN
                            }
                        }
                    },
                    new FilterGroup()
                    {
                        GroupKey = "jobs",
                        GroupName = stringLocalizer.GetString("AdvanceFilter.Jobs"),
                        Position = 3,
                        Properties = new List<Property>()
                        {
                            new Property()
                            {
                                Key = FilterPropertyKeyConstants.STAGES_OF_JOB,
                                Name = stringLocalizer.GetString("AdvanceFilter.StagesOfJob"),
                                Operators = Operator.CreateListByPropertyType(PropertyType.NESTED, stringLocalizer),
                                Type = PropertyType.NESTED,
                                FilterType = FilterType.RELATION
                            },
                            new Property()
                            {
                                Key = FilterPropertyKeyConstants.NUMBER_OF_APPLICATIONS,
                                Name = stringLocalizer.GetString("AdvanceFilter.NumberOfApplications"),
                                Operators = Operator.CreateListByPropertyType(PropertyType.NUMBER, stringLocalizer),
                                Type = PropertyType.NUMBER,
                                FilterType = FilterType.COLUMN
                            },
                            new Property()
                            {
                                Key = FilterPropertyKeyConstants.FIRST_APPLICATION_DATE,
                                Name = stringLocalizer.GetString("AdvanceFilter.FirstApplicationDate"),
                                Operators = Operator.CreateListByPropertyType(PropertyType.DATETIME, stringLocalizer),
                                Type = PropertyType.DATETIME,
                                FilterType = FilterType.COLUMN
                            },
                            new Property()
                            {
                                Key = FilterPropertyKeyConstants.LAST_APPLICATION_DATE,
                                Name = stringLocalizer.GetString("AdvanceFilter.LastApplicationDate"),
                                Operators = Operator.CreateListByPropertyType(PropertyType.DATETIME, stringLocalizer),
                                Type = PropertyType.DATETIME,
                                FilterType = FilterType.COLUMN
                            },
                            new Property()
                            {
                                Key = FilterPropertyKeyConstants.DISQUALIFICATION_REASONS,
                                Name = stringLocalizer.GetString("AdvanceFilter.DisqualificationReason"),
                                Operators = Operator.CreateListByPropertyType(PropertyType.LIST, stringLocalizer),
                                Type = PropertyType.LIST,
                                FilterType = FilterType.RELATION
                            }
                        }
                    },
                    new FilterGroup()
                    {
                        GroupKey = "interaction",
                        GroupName = stringLocalizer.GetString("AdvanceFilter.Interaction"),
                        Position = 4,
                        Properties = new List<Property>()
                        {
                            new Property()
                            {
                                Key = FilterPropertyKeyConstants.LAST_MEETING,
                                Name = stringLocalizer.GetString("AdvanceFilter.LastMeeting"),
                                Operators = Operator.CreateListByPropertyType(PropertyType.DATETIME, stringLocalizer),
                                Type = PropertyType.DATETIME,
                                FilterType = FilterType.COLUMN
                            },
                            new Property()
                            {
                                Key = FilterPropertyKeyConstants.LAST_CALL,
                                Name = stringLocalizer.GetString("AdvanceFilter.LastCall"),
                                Operators = Operator.CreateListByPropertyType(PropertyType.DATETIME, stringLocalizer),
                                Type = PropertyType.DATETIME,
                                FilterType = FilterType.COLUMN
                            },
                            new Property()
                            {
                                Key = FilterPropertyKeyConstants.LAST_EMAIL,
                                Name = stringLocalizer.GetString("AdvanceFilter.LastEmailSent"),
                                Operators = Operator.CreateListByPropertyType(PropertyType.DATETIME, stringLocalizer),
                                Type = PropertyType.DATETIME,
                                FilterType = FilterType.COLUMN
                            },
                            new Property()
                            {
                                Key = FilterPropertyKeyConstants.LAST_ACTIVITY,
                                Name = stringLocalizer.GetString("AdvanceFilter.LastActivityDate"),
                                Operators = Operator.CreateListByPropertyType(PropertyType.DATETIME, stringLocalizer),
                                Type = PropertyType.DATETIME,
                                FilterType = FilterType.COLUMN
                            }
                        }
                    }
                };
        }
    }

    public class FilterTag
    {
        public string Key { get; set; }
        public List<FilterTagValue> Values { get; set; }
    }

    public class FilterTagValue
    {
        public string Value { get; set; }

        public string Text { get; set; }

        public string Description { get; set; }

        public bool Checked { get; set; }

        public List<FilterTagValue> Children { get; set; }
    }

    public class Operator
    {
        public string Name { get; set; }
        public OperatorType Key { get; set; }

        public static List<Operator> CreateListByPropertyType(PropertyType propType, IStringLocalizer stringLocalizer)
        {
            var opList = new List<Operator>();

            switch (propType)
            {
                case PropertyType.STRING:
                    opList.AddRange(new List<Operator>()
                    {
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.Contains"),
                            Key = OperatorType.CONTAINS
                        },
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.DoesNotContains"),
                            Key = OperatorType.DOES_NOT_CONTAIN
                        },
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.StartsWith"),
                            Key = OperatorType.STARTS_WITH
                        },
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.EndsWith"),
                            Key = OperatorType.ENDS_WITH
                        },
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.HasAnyValue"),
                            Key = OperatorType.HAS_ANY_VALUE
                        },
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.IsUnknown"),
                            Key = OperatorType.IS_UNKNOWN
                        }
                    });
                    break;
                case PropertyType.NUMBER:
                    opList.AddRange(new List<Operator>()
                    {
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.GreaterThan"),
                            Key = OperatorType.GREATER_THAN
                        },
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.LessThan"),
                            Key = OperatorType.LESS_THAN
                        },
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.IsBetween"),
                            Key = OperatorType.IS_BETWEEN
                        },
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.Is"),
                            Key = OperatorType.IS
                        },
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.IsNot"),
                            Key = OperatorType.IS_NOT
                        },
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.HasAnyValue"),
                            Key = OperatorType.HAS_ANY_VALUE
                        },
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.IsUnknown"),
                            Key = OperatorType.IS_UNKNOWN
                        }
                    });
                    break;
                case PropertyType.DATETIME:
                    opList.AddRange(new List<Operator>()
                    {
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.MoreThan"),
                            Key = OperatorType.MORE_THAN
                        },
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.Exactly"),
                            Key = OperatorType.EXACTLY
                        },
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.LessThan"),
                            Key = OperatorType.LESS_THAN
                        },
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.After"),
                            Key = OperatorType.AFTER
                        },
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.On"),
                            Key = OperatorType.ON
                        },
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.Before"),
                            Key = OperatorType.BEFORE
                        },
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.HasAnyValue"),
                            Key = OperatorType.HAS_ANY_VALUE
                        },
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.IsUnknown"),
                            Key = OperatorType.IS_UNKNOWN
                        }
                    });
                    break;
                case PropertyType.LIST:
                    opList.AddRange(new List<Operator>()
                    {
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.IsAnyOf"),
                            Key = OperatorType.IS_ANY_OF
                        },
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.IsAllOf"),
                            Key = OperatorType.IS_ALL_OF
                        },
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.IsNoneOf"),
                            Key = OperatorType.IS_NONE_OF
                        },
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.HasAnyValue"),
                            Key = OperatorType.HAS_ANY_VALUE
                        },
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.IsUnknown"),
                            Key = OperatorType.IS_UNKNOWN
                        }
                    });
                    break;
                case PropertyType.BOOLEAN:
                    opList.AddRange(new List<Operator>()
                    {
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.HasAnyValue"),
                            Key = OperatorType.HAS_ANY_VALUE
                        },
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.IsUnknown"),
                            Key = OperatorType.IS_UNKNOWN
                        }
                    });
                    break;
                case PropertyType.FULLTXTSEARCH:
                    opList.AddRange(new List<Operator>()
                    {
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.Contains"),
                            Key = OperatorType.CONTAINS
                        }
                    });
                    break;
                case PropertyType.NESTED:
                    opList.AddRange(new List<Operator>()
                    {
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.IsAnyOf"),
                            Key = OperatorType.IS_ANY_OF
                        },
                        //new Operator()
                        //{
                        //    Name = "is all of",
                        //    Key = OperatorType.IS_ALL_OF
                        //},
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.IsNoneOf"),
                            Key = OperatorType.IS_NONE_OF
                        },
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.HasAnyValue"),
                            Key = OperatorType.HAS_ANY_VALUE
                        },
                        new Operator()
                        {
                            Name = stringLocalizer.GetString("AdvanceFilter.IsUnknown"),
                            Key = OperatorType.IS_UNKNOWN
                        }
                    });
                    break;
                default:
                    break;
            }

            return opList;
        }
    }

    public class Property
    {
        public Property()
        {
            Values = new List<string>();
        }

        public string Name { get; set; }
        public string Key { get; set; }
        public string Description { get; set; }
        public PropertyType Type { get; set; }
        public List<Operator> Operators { get; set; }

        public FilterType FilterType { get; set; }

        public List<string> Values { get; set; }
    }

    public static class FilterPropertyKeyConstants
    {
        public const string NAME = "name";
        public const string CURRENT_JOB_TITLE = "currentJobTitle";
        public const string CURRENT_COMPANY = "currentCompany";
        public const string SKILLS = "skills";
        public const string EMAILS = "emails";
        public const string PHONES = "phones";
        public const string LAST_MODIFIED = "lastModified";
        public const string CREATED = "created";
        public const string CANDIDATE_SOURCE = "candidateSource";
        public const string EDUCATIONS = "educations";
        public const string EXPERIENCES = "experiences";
        public const string AVATAAR = "avatar";
        public const string STAGES_OF_JOB = "stagesOfAJob";
        public const string ALL_JOB_STAGES = "allJobStages";
        public const string ALL_JOB_STAGE_TYPES = "allJobStageTypes";
        public const string HIRING_PIPELINE = "hiringPipeline";
        public const string NUMBER_OF_APPLICATIONS = "numberOfApplications";
        public const string NUMBER_OF_ACTIVE_APPLICATIONS = "numberOfActiveApplications";
        public const string FIRST_APPLICATION_DATE = "firstApplicationDate";
        public const string LAST_APPLICATION_DATE = "lastApplicationDate";
        public const string APPLICATION_SOURCES = "applicationSources";
        public const string DISQUALIFICATION_REASONS = "disqualificationReasons";
        public const string LAST_DISQUALIFICATION = "lastDisqualification";
        public const string POOL_STAGES = "poolStages";
        public const string ALL_POOL_STAGES = "allPoolStages";
        public const string ALL_POOL_STAGE_TYPES = "allPoolStageTypes";
        public const string POOL_PIPELINES = "poolPipelines";
        public const string CAMPAIGNS = "campaigns";
        public const string formSubmission = "formSubmissions";
        public const string SNOOZED_UNTIL = "snoozedUntil";
        public const string LAST_HEARD_FROM = "lastHeardFrom";
        public const string LAST_CONTACTED = "lastContacted";
        public const string LAST_MEETING = "lastMeeting";
        public const string LAST_CALL = "lastCall";
        public const string LAST_EMAIL = "lastEmail";
        public const string LAST_ACTIVITY = "lastActivity";
        public const string LANGUAGES = "languages";
        public const string JOBS = "jobs";
        public const string FULLTEXT_SEARCH = "fulltextSearch";
        public const string POOL = "pool";
        public const string TAGS = "tags";
        public const string ADDRESS = "Address";
    }

    #region Enums
    public enum FilterType
    {
        COLUMN,
        RELATION
    }

    public enum PropertyType
    {
        STRING,
        NUMBER,
        DATETIME,
        LIST,
        BOOLEAN,
        NESTED,
        FULLTXTSEARCH
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum OperatorType
    {
        IS_ANY_OF,
        IS_ALL_OF,
        IS_NONE_OF,
        HAS_ANY_VALUE,
        IS_UNKNOWN,
        EQUAL,
        NOT_EQUAL,
        GREATHER_THAN,
        LESS_THAN,
        BETWEEN,
        KNOWN,
        UNKNOWN,
        CONTAINS,
        NOT_CONTAINS,
        IN_RANGE,
        NOT_IN_RANGE,
        STARTS_WITH,
        ENDS_WITH,
        LIST_ANY_OF,
        LIST_ALL_OF,
        LIST_NONE_OF,
        IS_RELATIVE_EXACTLY,
        IS_RELATIVE_MORE_THAN,
        IS_RELATIVE_LESS_THAN,
        TRUE,
        FALSE,
        DAYS_AGO,
        IS,
        ON,
        IS_NOT,
        AFTER,
        GREATER_THAN,
        BEFORE,
        IS_BETWEEN,
        DOES_NOT_CONTAIN,
        IS_IN_RADIUS,
        IS_NOT_IN_RADIUS,
        EXACTLY,
        MORE_THAN,
        RELATIVE_LESS_THAN
    }
    #endregion

}
