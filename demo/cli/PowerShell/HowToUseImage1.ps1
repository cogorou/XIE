# XIE
# Copyright (C) 2013 Eggs Imaging Laboratory
# 
# How to use image objects.
# 

# Load assembly:
Add-Type -AssemblyName "XIE.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=5788d4539177400c"

# Initialize:
[XIE.Axi]::Setup()

# Create instance:
$src = New-Object XIE.CxImage
$src.Resize(640, 480, [XIE.TxModel]::U8(1), 3)
$src.ImageSize

# ----------------------------------------------------------------------
# Reset
$src.Reset()

# ----------------------------------------------------------------------
# clear pixels
$src.Clear(128.0)

# ----------------------------------------------------------------------
# clear pixels each channel
""
"----- clear pixels each channel"
for($ch=0 ; $ch -lt $src.Channels ; $ch++)
{
    $child = $src.Child($ch)
    $child.Clear(64.0 * ($ch + 1))
    $stat = $src.Statistics($ch)
    "channel={0}: Mean={1,3}: Sum1={2:N0}" -f $ch, $stat.Mean, $stat.Sum1
}

# ----------------------------------------------------------------------
# set value each pixel
""
"----- set value each pixel"

$src.Resize(100, 10, [XIE.TxModel]::U8(1), 3)
"ImageSize= {0}x{1} {2}({3}) x{4}ch" -f $src.Width, $src.Height, $src.Model.Type, $src.Model.Pack, $src.Channels
""

$watch = New-Object XIE.CxStopwatch
for($ch=0 ; $ch -lt $src.Channels ; $ch++)
{
    "channel={0}" -f $ch
    $scan = $src.Scanner($ch)

    $watch.Start()
    for($y=0 ; $y -lt $scan.Height ; $y++)
    {
        for($x=0 ; $x -lt $scan.Width ; $x++)
        {
            $pixel = [XIE.Ptr.BytePtr]($scan[$y, $x])
            $value = [System.Byte](Get-Random -Maximum 255 -Minimum 0)
            $pixel[0] = $value
        }
    }
    $watch.Stop()
    "Lap={0} msec" -f $watch.Lap

    $src.Statistics($ch)
}
"Elapsed={0} msec" -f $watch.Elapsed

exit
