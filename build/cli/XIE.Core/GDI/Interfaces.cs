/*
	XIE
	Copyright (C) 2013 Eggs Imaging Laboratory
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Reflection;

namespace XIE.GDI
{
	#region IxGdi2d

	/// <summary>
	/// �I�[�o���C�}�`�C���^�[�t�F�[�X (�Q�����p)
	/// </summary>
	public interface IxGdi2d
		: IxGdi2dRendering
		, IxGdi2dHandling
		, IxGdi2dVisualizing
	{
	}

	#endregion

	#region IxGdi2dRendering

	/// <summary>
	/// �I�[�o���C�}�`�`��C���^�[�t�F�[�X (�Q�����p)
	/// </summary>
	public interface IxGdi2dRendering
	{
		#region ���\�b�h:

		/// <summary>
		/// �}�`���O���t�B�N�X�ɕ`�悵�܂��B
		/// </summary>
		/// <param name="graphics">�`���̃O���t�B�N�X</param>
		/// <param name="info">�L�����o�X���</param>
		/// <param name="mode">�X�P�[�����O���[�h</param>
		void Render(Graphics graphics, CxCanvasInfo info, ExGdiScalingMode mode);
		
		#endregion
	}

	#endregion

	#region IxGdi2dHandling

	/// <summary>
	/// �I�[�o���C�}�`����C���^�[�t�F�[�X (�Q�����p)
	/// </summary>
	public interface IxGdi2dHandling : IxGdi2dRendering
	{
		#region �v���p�e�B:

		/// <summary>
		/// ��ʒu
		/// </summary>
		TxPointD Location { get; set; }

		/// <summary>
		/// �O�ڋ�`
		/// </summary>
		TxRectangleD Bounds { get; }

		/// <summary>
		/// ��]�p (degree) [�}180]
		/// </summary>
		double Angle { get; set; }

		/// <summary>
		/// ��]�̋@�� (��ʒu����̑��Βl(�}))
		/// </summary>
		TxPointD Axis { get; set; }

		#endregion

		#region ���\�b�h:

		/// <summary>
		/// �w����W���}�`�̂ǂ̈ʒu�ɂ��邩�𔻒肵�܂��B
		/// </summary>
		/// <param name="position">�w����W</param>
		/// <param name="margin">����̋��e�͈� [0~] ���}margin �͈͓̔��Ŕ��肵�܂��B</param>
		/// <returns>
		///		�w����W���}�`�O�ɂ���� 0 ��Ԃ��܂��B
		///		�w����W���}�`���ɂ���� 0 �ȊO�̒l��Ԃ��܂��B
		///		�Ԃ��l�͈̔͂͐}�`�ɂ���ĈقȂ�܂��B
		/// </returns>
		TxHitPosition HitTest(TxPointD position, double margin);

		/// <summary>
		/// �}�`�̕ҏW�i�ʒu�ړ��܂��͌`��ύX�j���s���܂��B
		/// </summary>
		/// <param name="prev_figure">�ҏW�O�̐}�`</param>
		/// <param name="prev_position">�ړ��O�̍��W</param>
		/// <param name="move_position">�ړ���̍��W</param>
		/// <param name="margin">����̋��e�͈� [0~] ���}margin �͈͓̔��Ŕ��肵�܂��B</param>
		void Modify(object prev_figure, TxPointD prev_position, TxPointD move_position, double margin);

		#endregion
	}

	#endregion

	#region IxGdi2dVisualizing

	/// <summary>
	/// �I�[�o���C�}�`���o���C���^�[�t�F�[�X (�Q�����p)
	/// </summary>
	public interface IxGdi2dVisualizing : IxGdi2dHandling
	{
		#region �v���p�e�B:

		/// <summary>
		/// �y���F
		/// </summary>
		TxRGB8x4 PenColor { get; set; }

		/// <summary>
		/// �y����
		/// </summary>
		float PenWidth { get; set; }

		/// <summary>
		/// �y���`��
		/// </summary>
		ExGdiPenStyle PenStyle { get; set; }

		/// <summary>
		/// �u���V�O�i�F
		/// </summary>
		TxRGB8x4 BrushColor { get; set; }

		/// <summary>
		/// �u���V�w�i�F
		/// </summary>
		TxRGB8x4 BrushShadow { get; set; }

		/// <summary>
		/// �u���V�X�^�C��
		/// </summary>
		ExGdiBrushStyle BrushStyle { get; set; }

		/// <summary>
		/// �n�b�`�X�^�C�� (Style �� Hatch �̎��Ɏg�p���܂��B����ȊO�͖������܂��B)
		/// </summary>
		System.Drawing.Drawing2D.HatchStyle HatchStyle { get; set; }

		/// <summary>
		/// ���`�O���f�[�V�������[�h (Style �� LinearGradient �̎��Ɏg�p���܂��B����ȊO�͖������܂��B)
		/// </summary>
		System.Drawing.Drawing2D.LinearGradientMode LinearGradientMode { get; set; }

		#endregion
	}

	#endregion
}
