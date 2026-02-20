using System.ComponentModel.DataAnnotations;

namespace WayPoint.Domain.Enums;

public enum VehicleType
{
    /// <summary>รถจักรยานยนต์ - ส่งด่วน พัสดุเล็ก/อาหาร ~30kg</summary>
    [Display(
        Name = "รถจักรยานยนต์",
        Description = "เน้นความรวดเร็ว พัสดุขนาดเล็ก หรืออาหาร",
        GroupName = "Express")]
    Motorcycle = 1,

    /// <summary>รถเก๋ง/5 ประตู - สินค้าต้องการดูแล ~200kg</summary>
    [Display(
        Name = "รถเก๋ง/รถ 5 ประตู",
        Description = "สินค้าที่ต้องการการดูแล หรือพื้นทู่จำกัด",
        GroupName = "Standard")]
    Sedan = 2,

    /// <summary>รถกระบะตู้ทึบ - ยอดนิยม ป้องกันแดด/ฝน 100% ~500kg</summary>
    [Display(
        Name = "รถกระบะตู้ทึบ",
        Description = "ยอดนิยมที่สุด ป้องกันแดดและฝน 100%",
        GroupName = "Standard")]
    PickupEnclosedVan = 3,

    /// <summary>รถกระบะคอก/โครงเหล็ก - สินค้าสูง/เกษตร ~1,000kg</summary>
    [Display(
        Name = "รถกระบะคอก/โครงเหล็ก",
        Description = "สำหรับสินค้าที่มีความสูง หรือสินค้าเกษตร",
        GroupName = "Standard")]
    PickupHighStructure = 4,

    /// <summary>รถ 4 ล้อใหญ่ (Jumbo) - ขนได้มาก วิ่งเมืองได้ ~2,000kg</summary>
    [Display(
        Name = "รถ 4 ล้อใหญ่ (Jumbo)",
        Description = "ขนได้มากกว่ากระบะ แต่ยังวิ่งได้ในเขตเมืองไม่ติดเวลา",
        GroupName = "Large")]
    FourWheelerJumbo = 5,

    /// <summary>รถบรรทุก 6 ล้อ - ขนย้ายบ้าน/อุตสาหกรรม ~5,000kg</summary>
    [Display(
        Name = "รถบรรทุก 6 ล้อ",
        Description = "เหมาะสำหรับการขนย้ายบ้าน หรือสินค้าอุตสาหกรรม",
        GroupName = "Large")]
    SixWheelerTruck = 6,

    /// <summary>รถบรรทุก 10 ล้อ+ - ขนส่งจำนวนมากระหว่างจังหวัด ~10,000kg+</summary>
    [Display(
        Name = "รถบรรทุก 10 ล้อขึ้นไป",
        Description = "เน้นขนส่งสินค้าปริมาณมากระหว่างจังหวัด",
        GroupName = "Large")]
    TenWheelerTruck = 7
}
