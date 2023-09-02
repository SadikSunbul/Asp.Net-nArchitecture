namespace ViabelliWebProject.Packages.Core.Persistance.Repositories;

/// <summary>
/// Entitylerin Zamansal olaylarını "ekleme,güncelleme,silme" olaylarını barındırır
/// </summary>
public interface IEntityTimeStamps
{
    DateTime CreateDate { get; set; }
    DateTime? UpdateDate { get; set; }
    DateTime? DeletedDate { get; set; }
}