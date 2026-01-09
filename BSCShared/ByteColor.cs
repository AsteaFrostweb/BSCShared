using System;

[Serializable]
public struct ByteColor
{
    public byte r, g, b, a;

    public ByteColor(byte r, byte g, byte b, byte a = 255)
    {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }

    private static float Clamp01(float value) => value < 0f ? 0f : (value > 1f ? 1f : value);

    public static ByteColor FromFloatColor(float r, float g, float b, float a = 1f)
        => new ByteColor(
            (byte)(Clamp01(r) * 255f),
            (byte)(Clamp01(g) * 255f),
            (byte)(Clamp01(b) * 255f),
            (byte)(Clamp01(a) * 255f)
        );

    public void ToFloatColor(out float r, out float g, out float b, out float a)
    {
        r = this.r / 255f;
        g = this.g / 255f;
        b = this.b / 255f;
        a = this.a / 255f;
    }

    public static ByteColor Lerp(ByteColor a, ByteColor b, float t)
    {
        t = Clamp01(t);
        return new ByteColor(
            (byte)(a.r + (b.r - a.r) * t),
            (byte)(a.g + (b.g - a.g) * t),
            (byte)(a.b + (b.b - a.b) * t),
            (byte)(a.a + (b.a - a.a) * t)
        );
    }

    public override string ToString() => $"({r},{g},{b},{a})";

    public static readonly ByteColor White = new ByteColor(255, 255, 255, 255);
    public static readonly ByteColor Black = new ByteColor(0, 0, 0, 255);
}