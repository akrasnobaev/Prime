using System;
using System.Linq.Expressions;
using OptimusPrime.FuncLibertyPrime;

namespace Prime
{
    public class FuncLibertyFactory : LibertyFactory
    {
        public override IChain<TIn, TOut> CreateChain<TIn, TOut>(Func<TIn, TOut> function, string pseudoName = null)
        {
            var outputName = ServiceNameHelper.GetCollectionName<TOut>();
            // Если указан псевдоним, добавляем его в коллекцию псевдонимов имен.
            if (!string.IsNullOrEmpty(pseudoName))
                pseudoNames.Add(pseudoName, outputName);

            // Параметр "inputData".
            var parameter = Expression.Parameter(typeof (TIn), "inputData");

            // Вызов функтора function.
            Expression<Func<TIn, TOut>> bind = inputData => function(inputData);
            var invoke = Expression.Invoke(bind, parameter);

            // Собираем Clone.
            var smartCloneType = typeof (SmartClone<TOut>);
            var smartClone = Expression.New(smartCloneType);
            var clone = Expression.MemberInit(smartClone);

            // Вызываем Clone после на результате работы функтора.
            var call = Expression.Call(clone,
                smartCloneType.GetMethod("Clone", new[] {typeof (TOut)}),
                invoke);
            var lambdaCall = Expression.Lambda<Func<TIn, TOut>>(call, parameter);

            return new FuncLibertyChain<TIn, TOut>(this, lambdaCall, outputName);
        }

        public override IChain<TIn, TOut> LinkChainToChain<TIn, TOut, TMiddle>(IChain<TIn, TMiddle> first,
            IChain<TMiddle, TOut> second)
        {
            // Помечаем исходные цепочки как использованные.
            first.MarkUsed();
            second.MarkUsed();

            // Используем небезопасное кастование, чтобы исключение указало на правильное место.
            var firstChain = (IFuncLibertyChain<TIn, TMiddle>) first;
            var secondChain = (IFuncLibertyChain<TMiddle, TOut>) second;

            var firstExpresion = firstChain.Expression;
            var secondExpression = secondChain.Expression;

            var body = SwapVisitor.Swap(secondExpression.Body, secondExpression.Parameters[0],
                firstExpresion.Body);
            var lambda = Expression.Lambda<Func<TIn, TOut>>(body, firstExpresion.Parameters);
            return new FuncLibertyChain<TIn, TOut>(this, lambda, secondChain.OutputName);
        }
    }

    internal class SwapVisitor : ExpressionVisitor
    {
        private readonly Expression from, to;

        private SwapVisitor(Expression from, Expression to)
        {
            this.from = from;
            this.to = to;
        }

        public static Expression Swap(Expression body,
            Expression from, Expression to)
        {
            return new SwapVisitor(from, to).Visit(body);
        }

        public override Expression Visit(Expression node)
        {
            return node == from ? to : base.Visit(node);
        }
    }
}