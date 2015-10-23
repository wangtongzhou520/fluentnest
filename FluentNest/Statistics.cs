﻿using System;
using System.Linq.Expressions;
using Nest;

namespace FluentNest
{
    public static class Statistics
    {
        public static AggregationDescriptor<T> AndSumBy<T>(this AggregationDescriptor<T> agg, Expression<Func<T, Object>> fieldGetter) where T : class
        {
            return agg.Sum(fieldGetter.GetName(), x => x.Field(fieldGetter));
        }

        public static AggregationDescriptor<T> AndCountBy<T>(this AggregationDescriptor<T> agg, Expression<Func<T, Object>> fieldGetter) where T : class
        {
            return agg.ValueCount(fieldGetter.GetName(), x => x.Field(fieldGetter));
        }

        public static AggregationDescriptor<T> AndCardinalityBy<T>(this AggregationDescriptor<T> agg, Expression<Func<T, Object>> fieldGetter) where T : class
        {
            return agg.Cardinality(fieldGetter.GetName(), x => x.Field(fieldGetter));
        }

        public static AggregationDescriptor<T> AndCondCountBy<T>(this AggregationDescriptor<T> agg, Expression<Func<T, object>> fieldGetter, Expression<Func<T, bool>> filterRule) where T : class
        {
            var fieldName = fieldGetter.GetName();
            var filterName = filterRule.GetFieldNameFromAccessor();
            agg.Filter(filterName,
                f =>
                    f.Filter(fd => filterRule.Body.GenerateFilterDescription<T>())
                        .Aggregations(innerAgg => innerAgg.ValueCount(fieldName, field => field.Field(fieldGetter))));
            return agg;
        }

        public static DateHistogramAggregationDescriptor<T> SumBy<T>(this DateHistogramAggregationDescriptor<T> agg, Expression<Func<T, Object>> fieldGetter) where T : class
        {
            return agg.Aggregations(x => x.Sum(fieldGetter.GetName(), dField => dField.Field(fieldGetter)));
        }

        public static AggregationDescriptor<T> SumBy<T>(Expression<Func<T, object>> fieldGetter) where T : class
        {
            AggregationDescriptor<T> v = new AggregationDescriptor<T>();
            var fieldName = fieldGetter.GetName();
            var sumAggs = v.Sum(fieldName, tr => tr.Field(fieldGetter));
            return sumAggs;
        }

        public static AggregationDescriptor<T> CardinalityBy<T>(Expression<Func<T, object>> fieldGetter) where T : class
        {
            AggregationDescriptor<T> v = new AggregationDescriptor<T>();
            var fieldName = fieldGetter.GetName();
            var sumAggs = v.Cardinality(fieldName, tr => tr.Field(fieldGetter));
            return sumAggs;
        }

        public static AggregationDescriptor<T> CountBy<T>(Expression<Func<T, object>> fieldGetter) where T : class
        {
            AggregationDescriptor<T> v = new AggregationDescriptor<T>();
            var fieldName = fieldGetter.GetName();
            var sumAggs = v.ValueCount(fieldName, tr => tr.Field(fieldGetter));
            return sumAggs;
        }

        public static AggregationDescriptor<T> CondSumBy<T>(Expression<Func<T, object>> fieldGetter, Expression<Func<T, bool>> filterRule) where T : class
        {
            AggregationDescriptor<T> v = new AggregationDescriptor<T>();
            var fieldName = fieldGetter.GetName();
            var filterName = filterRule.GetFieldNameFromAccessor();
            var filtered = v.Filter(filterName,
                f =>
                    f.Filter(fd => filterRule.Body.GenerateFilterDescription<T>())
                        .Aggregations(innerAgg => innerAgg.Sum(fieldName, field => field.Field(fieldGetter))));
            return filtered;
        }

        public static AggregationDescriptor<T> AndCondSumBy<T>(this AggregationDescriptor<T> agg, Expression<Func<T, object>> fieldGetter, Expression<Func<T, bool>> filterRule) where T : class
        {
            var fieldName = fieldGetter.GetName();
            var filterName = filterRule.GetFieldNameFromAccessor();
            agg.Filter(filterName,
                f =>
                    f.Filter(fd => filterRule.Body.GenerateFilterDescription<T>())
                        .Aggregations(innerAgg => innerAgg.Sum(fieldName, field => field.Field(fieldGetter))));
            return agg;
        }

        public static AggregationDescriptor<T> CondCountBy<T>(Expression<Func<T, object>> fieldGetter, Expression<Func<T, bool>> filterRule) where T : class
        {
            AggregationDescriptor<T> v = new AggregationDescriptor<T>();
            var fieldName = fieldGetter.GetName();
            var filterName = filterRule.GetFieldNameFromAccessor();
            var filtered = v.Filter(filterName,
                f =>
                    f.Filter(fd => filterRule.Body.GenerateFilterDescription<T>())
                        .Aggregations(innerAgg => innerAgg.ValueCount(fieldName, field => field.Field(fieldGetter))));
            return filtered;
        }

        public static AggregationDescriptor<T> DistinctBy<T>(Expression<Func<T, Object>> fieldGetter) where T : class
        {
            AggregationDescriptor<T> v = new AggregationDescriptor<T>();
            var fieldName = fieldGetter.GetName();
            v.Terms(fieldName, tr =>
            {
                TermsAggregationDescriptor<T> trmAggDescriptor = new TermsAggregationDescriptor<T>();
                trmAggDescriptor.Field(fieldGetter);
                return trmAggDescriptor;
            });

            return v;
        }

        public static AggregationDescriptor<T> AndDistinctBy<T>(this AggregationDescriptor<T> agg, Expression<Func<T, Object>> fieldGetter) where T : class
        {
            var fieldName = fieldGetter.GetName();
            return agg.Terms(fieldName, x => x.Field(fieldGetter));
        }

        public static AggregationDescriptor<T> AndAvgBy<T>(this AggregationDescriptor<T> agg, Expression<Func<T, Object>> fieldGetter) where T : class
        {
            return agg.Average(fieldGetter.GetName(), x => x.Field(fieldGetter));
        }
    }
}
