namespace Stinkhorn.Bureau.Context
{
    public interface IAddressBook
    {
        void AddOrUpdate(Contact contact);
    }

    public interface IAddresser
    {
        IAddressBook Book { set; }
    }
}