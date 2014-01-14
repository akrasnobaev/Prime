namespace Prime.Optimus
{
    public interface IOptimusOut
    {
        string Name { get; }
        IOptimusService Service { get; }

        void Set(object value);
    }
}