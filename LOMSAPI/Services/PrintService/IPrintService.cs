namespace LOMSAPI.Services
{
    public interface IPrintService
    {
        public void PrintCustomerLabel(string comPort, PrintInfo info);
    }
}
