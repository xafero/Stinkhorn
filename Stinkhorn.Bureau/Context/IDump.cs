namespace Stinkhorn.Bureau.Context
{
    public interface IDump
    {
        void Receive<T>(T msg, object title = null);
    }

    public interface IDumper
    {
        IDump Dump { set; }
    }
}