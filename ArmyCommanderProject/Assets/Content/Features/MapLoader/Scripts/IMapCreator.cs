namespace Content.Features.MapLoader.Scripts
{
    public interface IMapCreator
    {
        //находим и спавним активную карту, может загружаем с сохранения
        public void InitMap();
        public void InitMapById(int id);
    }
}