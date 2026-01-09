using System.Threading.Tasks;

public interface IRegionIO
{
    Task<Region> LoadRegionAsync(RegionCoord coord);
    void SaveRegion(Region region);
}
