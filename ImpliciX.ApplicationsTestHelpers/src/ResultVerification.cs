using ImpliciX.Language.Core;
using NFluent;
using NFluent.Extensibility;

namespace ImpliciX.ApplicationsTestHelpers
{
    public static class ResultVerification
    {
        public static ICheckLink<ICheck<Result<T>>> IsSuccess<T>(this ICheck<Result<T>> check)
        {
            ExtensibilityHelper.BeginCheck(check)
                .FailWhen(x => x.IsError, "The {0} is error whereas it must be success.")
                .OnNegate("The {0} is success whereas it must be error.")
                .EndCheck();
            return ExtensibilityHelper.BuildCheckLink(check);
        }

        public static ICheckLink<ICheck<Result<T>>> IsError<T>(this ICheck<Result<T>> check)
        {
            return check.Not.IsSuccess();
        }

        public static ICheck<T> ResultingValue<T>(this ICheck<Result<T>> check)
        {
            var result = ExtensibilityHelper.ExtractChecker(check).Value;
            return Check.That(result.Value);
        }

        public static ICheck<Error> ResultingError<T>(this ICheck<Result<T>> check)
        {
            var result = ExtensibilityHelper.ExtractChecker(check).Value;
            return Check.That(result.Error);
        }
    }
}