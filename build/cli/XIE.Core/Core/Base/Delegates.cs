/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XIE
{
	/// <summary>
	/// １次元配列の各要素に対して実行する関数のデリゲート
	/// </summary>
	/// <param name="x">現在の指標 [0~]</param>
	/// <param name="addr1">現在のアドレス</param>
	public delegate void Scan1D1(int x, IntPtr addr1);

	/// <summary>
	/// １次元配列の各要素に対して実行する関数のデリゲート
	/// </summary>
	/// <param name="x">現在の指標 [0~]</param>
	/// <param name="addr1">現在のアドレス (１つ目の配列)</param>
	/// <param name="addr2">現在のアドレス (２つ目の配列)</param>
	public delegate void Scan1D2(int x, IntPtr addr1, IntPtr addr2);

	/// <summary>
	/// １次元配列の各要素に対して実行する関数のデリゲート
	/// </summary>
	/// <param name="x">現在の指標 [0~]</param>
	/// <param name="addr1">現在のアドレス (１つ目の配列)</param>
	/// <param name="addr2">現在のアドレス (２つ目の配列)</param>
	/// <param name="addr3">現在のアドレス (３つ目の配列)</param>
	public delegate void Scan1D3(int x, IntPtr addr1, IntPtr addr2, IntPtr addr3);

	/// <summary>
	/// １次元配列の各要素に対して実行する関数のデリゲート
	/// </summary>
	/// <param name="x">現在の指標 [0~]</param>
	/// <param name="addr1">現在のアドレス (１つ目の配列)</param>
	/// <param name="addr2">現在のアドレス (２つ目の配列)</param>
	/// <param name="addr3">現在のアドレス (３つ目の配列)</param>
	/// <param name="addr4">現在のアドレス (４つ目の配列)</param>
	public delegate void Scan1D4(int x, IntPtr addr1, IntPtr addr2, IntPtr addr3, IntPtr addr4);

	/// <summary>
	/// １次元配列の各要素に対して実行する関数のデリゲート
	/// </summary>
	/// <param name="x">現在の指標 [0~]</param>
	/// <param name="addr1">現在のアドレス (１つ目の配列)</param>
	/// <param name="addr2">現在のアドレス (２つ目の配列)</param>
	/// <param name="addr3">現在のアドレス (３つ目の配列)</param>
	/// <param name="addr4">現在のアドレス (４つ目の配列)</param>
	/// <param name="addr5">現在のアドレス (５つ目の配列)</param>
	public delegate void Scan1D5(int x, IntPtr addr1, IntPtr addr2, IntPtr addr3, IntPtr addr4, IntPtr addr5);

	/// <summary>
	/// １次元配列の各要素に対して実行する関数のデリゲート
	/// </summary>
	/// <param name="x">現在の指標 [0~]</param>
	/// <param name="addr1">現在のアドレス (１つ目の配列)</param>
	/// <param name="addr2">現在のアドレス (２つ目の配列)</param>
	/// <param name="addr3">現在のアドレス (３つ目の配列)</param>
	/// <param name="addr4">現在のアドレス (４つ目の配列)</param>
	/// <param name="addr5">現在のアドレス (５つ目の配列)</param>
	/// <param name="addr6">現在のアドレス (６つ目の配列)</param>
	public delegate void Scan1D6(int x, IntPtr addr1, IntPtr addr2, IntPtr addr3, IntPtr addr4, IntPtr addr5, IntPtr addr6);

	/// <summary>
	/// ２次元配列の各要素に対して実行する関数のデリゲート
	/// </summary>
	/// <param name="y">現在の Y 座標 [0~]</param>
	/// <param name="x">現在の X 座標 [0~]</param>
	/// <param name="addr1">現在のアドレス</param>
	public delegate void Scan2D1(int y, int x, IntPtr addr1);

	/// <summary>
	/// ２次元配列の各要素に対して実行する関数のデリゲート
	/// </summary>
	/// <param name="y">現在の Y 座標 [0~]</param>
	/// <param name="x">現在の X 座標 [0~]</param>
	/// <param name="addr1">現在のアドレス (１つ目の配列)</param>
	/// <param name="addr2">現在のアドレス (２つ目の配列)</param>
	public delegate void Scan2D2(int y, int x, IntPtr addr1, IntPtr addr2);

	/// <summary>
	/// ２次元配列の各要素に対して実行する関数のデリゲート
	/// </summary>
	/// <param name="y">現在の Y 座標 [0~]</param>
	/// <param name="x">現在の X 座標 [0~]</param>
	/// <param name="addr1">現在のアドレス (１つ目の配列)</param>
	/// <param name="addr2">現在のアドレス (２つ目の配列)</param>
	/// <param name="addr3">現在のアドレス (３つ目の配列)</param>
	public delegate void Scan2D3(int y, int x, IntPtr addr1, IntPtr addr2, IntPtr addr3);

	/// <summary>
	/// ２次元配列の各要素に対して実行する関数のデリゲート
	/// </summary>
	/// <param name="y">現在の Y 座標 [0~]</param>
	/// <param name="x">現在の X 座標 [0~]</param>
	/// <param name="addr1">現在のアドレス (１つ目の配列)</param>
	/// <param name="addr2">現在のアドレス (２つ目の配列)</param>
	/// <param name="addr3">現在のアドレス (３つ目の配列)</param>
	/// <param name="addr4">現在のアドレス (４つ目の配列)</param>
	public delegate void Scan2D4(int y, int x, IntPtr addr1, IntPtr addr2, IntPtr addr3, IntPtr addr4);

	/// <summary>
	/// ２次元配列の各要素に対して実行する関数のデリゲート
	/// </summary>
	/// <param name="y">現在の Y 座標 [0~]</param>
	/// <param name="x">現在の X 座標 [0~]</param>
	/// <param name="addr1">現在のアドレス (１つ目の配列)</param>
	/// <param name="addr2">現在のアドレス (２つ目の配列)</param>
	/// <param name="addr3">現在のアドレス (３つ目の配列)</param>
	/// <param name="addr4">現在のアドレス (４つ目の配列)</param>
	/// <param name="addr5">現在のアドレス (５つ目の配列)</param>
	public delegate void Scan2D5(int y, int x, IntPtr addr1, IntPtr addr2, IntPtr addr3, IntPtr addr4, IntPtr addr5);

	/// <summary>
	/// ２次元配列の各要素に対して実行する関数のデリゲート
	/// </summary>
	/// <param name="y">現在の Y 座標 [0~]</param>
	/// <param name="x">現在の X 座標 [0~]</param>
	/// <param name="addr1">現在のアドレス (１つ目の配列)</param>
	/// <param name="addr2">現在のアドレス (２つ目の配列)</param>
	/// <param name="addr3">現在のアドレス (３つ目の配列)</param>
	/// <param name="addr4">現在のアドレス (４つ目の配列)</param>
	/// <param name="addr5">現在のアドレス (５つ目の配列)</param>
	/// <param name="addr6">現在のアドレス (６つ目の配列)</param>
	public delegate void Scan2D6(int y, int x, IntPtr addr1, IntPtr addr2, IntPtr addr3, IntPtr addr4, IntPtr addr5, IntPtr addr6);
}
