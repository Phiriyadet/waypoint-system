using WayPoint.Domain.Exceptions;

namespace WayPoint.Domain.ValueObjects;

/// <summary>
/// Value Object ของที่อยู่แบบ structured สำหรับระบบไทย
/// ใช้ sealed record — equality เปรียบเทียบจากทุก field อัตโนมัติ
/// </summary>
public sealed record Address
{
    /// <summary>บ้านเลขที่ / ซอย / ถนน / ชั้น / ห้อง</summary>
    public string AddressInfo { get; }

    /// <summary>แขวง / ตำบล</summary>
    public string Subdistrict { get; }

    /// <summary>เขต / อำเภอ</summary>
    public string District { get; }

    /// <summary>จังหวัด</summary>
    public string Province { get; }

    /// <summary>รหัสไปรษณีย์ 5 หลัก</summary>
    public string PostalCode { get; }

    /// <summary>ข้อมูลเพิ่มเติมสำหรับ rider เช่น "ตึก A ชั้น 3 ห้อง 302" (optional)</summary>
    public string? MoreInfo { get; }

    private Address(string addressInfo, string subdistrict,
        string district, string province, string postalCode, string? moreInfo)
    {
        AddressInfo = addressInfo; Subdistrict = subdistrict;
        District = district; Province = province;
        PostalCode = postalCode; MoreInfo = moreInfo;
    }

    /// <summary>สร้าง Address พร้อม validate ทุก required field</summary>
    public static Address Create(
        string addressInfo, string subdistrict, string district,
        string province, string postalCode, string? moreInfo = null)
    {
        if (string.IsNullOrWhiteSpace(addressInfo)) throw new ArgumentException("AddressInfo is required.");
        if (string.IsNullOrWhiteSpace(subdistrict)) throw new ArgumentException("Subdistrict is required.");
        if (string.IsNullOrWhiteSpace(district)) throw new ArgumentException("District is required.");
        if (string.IsNullOrWhiteSpace(province)) throw new ArgumentException("Province is required.");
        if (string.IsNullOrWhiteSpace(postalCode) || postalCode.Length != 5 || !postalCode.All(char.IsDigit))
            throw new InvalidCoordinateException("PostalCode must be exactly 5 digits.");
        return new Address(addressInfo.Trim(), subdistrict.Trim(),
            district.Trim(), province.Trim(), postalCode.Trim(), moreInfo?.Trim());
    }

    /// <summary>คืนที่อยู่สำหรับส่งไป geocoder — ไม่รวม MoreInfo</summary>
    public string ToGeocodingString() =>
        $"{AddressInfo} {Subdistrict} {District} {Province} {PostalCode}";

    /// <summary>คืนที่อยู่เต็มรวม MoreInfo สำหรับแสดงให้ rider เห็น</summary>
    public string ToFullAddress()
    {
        var base_ = $"{AddressInfo} {Subdistrict} {District} {Province} {PostalCode}";
        return string.IsNullOrWhiteSpace(MoreInfo) ? base_ : $"{base_} ({MoreInfo})";
    }
}