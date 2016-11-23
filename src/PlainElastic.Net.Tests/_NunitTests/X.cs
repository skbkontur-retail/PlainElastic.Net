using System;
using System.Linq.Expressions;
using System.Reflection;
using PlainElastic.Net.Mappings;

namespace PlainElastic.Net.Tests._NunitTests
{
    public static class X
    {
        public static Properties<T> NotAnalyzedStringWithAnalazedCopyForEs1X<T, TProperty>(this Properties<T> properties, Expression<Func<T, TProperty>> picker)
        {
            return properties.MultiField(picker,
                mf => mf
                    .Fields(fs => fs.String(picker, x => x.Index(IndexState.not_analyzed))
                        .String(FieldName(picker) + "_analyzed", x => x.Index(IndexState.analyzed)))
                );
        }

        public static Properties<T> NotAnalyzedStringWithAnalazedCopyForEs5X<T, TProperty>(this Properties<T> properties, Expression<Func<T, TProperty>> picker)
        {
            return properties.String(picker, mf => mf
                .Index(IndexState.not_analyzed)
                .Fields(fs => fs.String(FieldName(picker) + "_analyzed", x => x.Index(IndexState.analyzed))));
        }

        private static string FieldName<T, TProperty>(Expression<Func<T, TProperty>> picker)
        {
            return ((picker.Body as MemberExpression).Member as PropertyInfo).Name;
        }
    }
}