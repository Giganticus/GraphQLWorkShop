using HotChocolate.Types;

namespace ConferencePlanner.GraphQL.Extensions;

public static class MyObjectFieldDescriptorExtensions
{
    public static IObjectFieldDescriptor UseUpperCase(
        this IObjectFieldDescriptor descriptor)
    {
        return descriptor.Use(next => async context =>
        {
            await next(context);

            if (context.Result is string s)
            {
                context.Result = s.ToUpperInvariant();
            }
        });
    }
}