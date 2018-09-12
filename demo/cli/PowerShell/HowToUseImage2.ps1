# XIE
# Copyright (C) 2013 Eggs Imaging Laboratory
# 
# How to use image objects and OpenFileDialog.
# 

# Load assembly:
Add-Type -AssemblyName "System.Windows.Forms"
Add-Type -AssemblyName "XIE.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=5788d4539177400c"

# Initialize:
[XIE.Axi]::Setup()

# OpenFileDialog クラスのインスタンス生成:
$dlg = New-Object System.Windows.Forms.OpenFileDialog
$dlg.Title = "Please select the required file."
$dlg.Filter = "Image Files(*.bmp;*.png;*.jpeg;*.tiff)|*.bmp;*.png;*.jpg;*.jpeg;*.tif;*.tiff;"
$dlg.Multiselect = $TRUE
$dlg.CheckFileExists = $TRUE
$dlg.CheckPathExists = $TRUE
$dlg.SupportMultiDottedExtensions = $TRUE
# $dlg.InitialDirectory = "C:\"

# OpenFileDialog の表示:
$ans = $dlg.ShowDialog()
if ($ans -eq "OK") {
  # ［OK］ボタンが押下された時:
  foreach($filename in $dlg.FileNames) {
    $filename
    $src = New-Object XIE.CxImage($filename)
    "Size    ={0}" -f $src.Size
    "Model   ={0}" -f $src.Model
    "Channels={0}" -f $src.Channels
    "Depth   ={0}" -f $src.Depth
  }
}

exit
