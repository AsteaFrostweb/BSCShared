using System.Threading.Tasks;

public interface IRegionLoader
{
    Task<Region> LoadRegionAsync(RegionCoord coord);
    void UnloadRegion(RegionCoord coord);
    void MarkDirty(RegionCoord coord);
}
