# XIE
# Copyright (C) 2013 Eggs Imaging Laboratory
# 
# 複数の画像ファイルを読み込んでサムネイルを生成します。
# 

# アセンブリのロード:
Add-Type -AssemblyName "System.Windows.Forms"
Add-Type -AssemblyName "XIE.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=5788d4539177400c"

# 初期化:
[XIE.Axi]::Setup()

# OpenFileDialog クラスのインスタンス生成:
$ofd = New-Object System.Windows.Forms.OpenFileDialog
$ofd.Title = "Please select the required file."
$ofd.Filter = "Image Files(*.bmp;*.png;*.jpeg;*.tiff)|*.bmp;*.png;*.jpg;*.jpeg;*.tif;*.tiff;"
$ofd.Multiselect = $TRUE
$ofd.CheckFileExists = $TRUE
$ofd.CheckPathExists = $TRUE
# $ofd.InitialDirectory = "C:\"

# OpenFileDialog の表示:
$ofd_result = $ofd.ShowDialog()
if ($ofd_result -eq "OK") {
    # ［OK］ボタンが押下された時:

    $images = New-Object "System.Collections.Generic.List[XIE.CxImage]"

    # -----------------------------------------------------------------
    # 全てのファイルを読み込みます.
    $ofd.FileNames | ForEach-Object {
        $src = New-Object XIE.CxImage
        $src.Load($_)
        $images.Add($src)

        "{0,5}x{1,5} pix, {2,2} bits : {3}" -f $src.Width, $src.Height, $src.Depth, $_

        if ($src.Model.Type -ne [XIE.ExType]::U8) {
            $scale_value = [XIE.Axi]::CalcScale($src.Model.Type, $src.Depth, [XIE.ExType]::U8, 8)
            $src.Filter().Mul($src, $scale_value)
            $src.Depth = 8
        }
        elseif ($src.Depth -ne 0 -and $src.Depth -lt 8) {
            $scale_value = [XIE.Axi]::CalcScale($src.Model.Type, $src.Depth, [XIE.ExType]::U8, 8)
            $src.Filter().Mul($src, $scale_value)
            $src.Depth = 8
        }
    }

    # -----------------------------------------------------------------
    # ファイル数に応じて行列数を決定します.
    "count={0}" -f $images.Count

    $rows = 1
    $cols = 1

    if ($images.Count -le 1) {
        $cols = 1
        $rows = 1
    }
    elseif ($images.Count -le 4) {
        $cols = 2
        $rows = [int][System.Math]::Ceiling($images.Count / $cols)
    }
    else {
        $cols = 3
        $rows = [int][System.Math]::Ceiling($images.Count / $cols)
    }

    "rows={0}" -f $rows
    "cols={0}" -f $cols

    # -----------------------------------------------------------------
    # 行の合計幅の最大値、各行の最大の高さの合計値を計算します.
    $sum_width = 0
    $sum_height = 0

    $index = 0
    for($r=0 ; $r -lt $rows ; $r++) {
        if (-not ($index -lt $images.Count)) { break }
        $sum = 0
        $max_height = 0
        for($c=0 ; $c -lt $cols ; $c++) {
            if (-not ($index -lt $images.Count)) { break }
            $item = $images[$index]
            $sum += $item.Width
            if ($max_height -lt $item.Height) {
                $max_height = $item.Height
            }
            $index++
        }
        if ($sum_width -lt $sum) {
            $sum_width = $sum
        }
        $sum_height += $max_height
    }

    "sum_width ={0:N0}" -f $sum_width
    "sum_height={0:N0}" -f $sum_height

    # -----------------------------------------------------------------
    # 縮小率を計算します.
    $limit_size = 2048
    $margin = 16

    $mag_x = ($limit_size - ($margin * $cols + $margin)) / $sum_width
    $mag_y = ($limit_size - ($margin * $rows + $margin)) / $sum_height

    $mag = 1.0
    if ($mag_x -lt $mag_y -and $mag_x -lt 1.0) {
        $mag = $mag_x
    }
    elseif ($mag_y -lt $mag_x -and $mag_y -lt 1.0) {
        $mag = $mag_y
    }

    "mag_x={0:F}" -f $mag_x
    "mag_y={0:F}" -f $mag_y
    "mag  ={0:F}" -f $mag

    # -----------------------------------------------------------------
    # サムネイルを生成します.

    $interpolation = [int]2
    if ($mag -ge 1.0) {
        $interpolation = [int]0
    }
    elseif ($mag -gt 0.5) {
        $interpolation = [int]1
    }

    $dst_width = [int][System.Math]::Round($sum_width * $mag + ($margin * $cols + $margin))
    $dst_height = [int][System.Math]::Round($sum_height * $mag + ($margin * $rows + $margin))

    "dst_width ={0:N0}" -f $dst_width
    "dst_height={0:N0}" -f $dst_height

    $dst = New-Object XIE.CxImage
    $dst.Resize($dst_width, $dst_height, [XIE.TxModel]::U8(4), 1);
    $dst.Reset();

    $roi = New-Object XIE.CxImage

    $roi_y = $margin
    $index = 0
    for($r=0 ; $r -lt $rows ; $r++) {
        if (-not ($index -lt $images.Count)) { break }
        $roi_x = $margin
        $max_height = 0
        for($c=0 ; $c -lt $cols ; $c++) {
            if (-not ($index -lt $images.Count)) { break }
            $item = $images[$index]

            # ROI を設定して縮小します.
            $roi_width = [int][System.Math]::Round($item.Width * $mag)
            $roi_height = [int][System.Math]::Round($item.Height * $mag)
            $region = New-Object XIE.TxRectangleI($roi_x, $roi_y, $roi_width, $roi_height)
            $roi.Attach($dst, $region)
            $roi.Filter().Scale($item, $mag, $mag, $interpolation)

            # ROI の開始位置(水平方向)を移動します.
            $roi_x += ($roi_width + $margin)
            if ($max_height -lt $roi_height) {
                $max_height = $roi_height
            }
            $index++
        }
        # ROI の開始位置(垂直方向)を移動します.
        $roi_y += ($max_height + $margin)
    }

    $timestamp = Get-Date -Format "yyyyMMdd_hhmmss_fff"
    $dst_filename = "Thumbnail_{0}.png" -f $timestamp

    # SaveFileDialog クラスのインスタンス生成:
    $sfd = New-Object System.Windows.Forms.SaveFileDialog
    $sfd.Title = "Please specifiy the save destination file name."
    $sfd.Filter = "Image Files(*.bmp;*.png;*.jpeg;*.tiff)|*.bmp;*.png;*.jpg;*.jpeg;*.tif;*.tiff;"
    $sfd.FileName = $dst_filename

    # SaveFileDialog の表示:
    $sfd_result = $sfd.ShowDialog()
    if ($sfd_result -eq "OK") {
        # ［OK］ボタンが押下された時:
        $dst.Save($sfd.FileName)
    }
}

exit
