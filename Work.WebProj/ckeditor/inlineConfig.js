/**
 * @license Copyright (c) 2003-2015, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	 config.language = 'zh';
    // config.uiColor = '#AADC6E';

    config.contentsCss = ['../Content/css/editor.css'];
    config.toolbar = [
        { name: "document", items: [ "Source", "-" ] },
        { name: "tools", items: ["Maximize", "-"]},
        { name: "clipboard",
          items: ["Cut", "Copy", "Paste", "PasteText", "PasteFromWord", "Undo", "Redo"]
        },
        { name: "links", items: ["Link", "Unlink", "Anchor"] },
        { name: "basicstyles",
          items: ["FontSize", "Bold", "Underline", "Strike", "-", "JustifyLeft", "JustifyCenter", "JustifyRight", "-", "RemoveFormat"]
        },
        '/',
        { name: "paragraph", items: ["NumberedList", "BulletedList","-","Outdent","Indent"] },
        { name: "colors", items: ["TextColor", "BGColor"] },
        { name: "styles", items: ["Styles", "Format"] },
        { name: "editing" }
    ];
    config.filebrowserBrowseUrl = "../ckfinder/ckfinder.html";
    config.filebrowserImageBrowseUrl = "../ckfinder/ckfinder.html?type=Images";
    config.filebrowserImageUploadUrl = "../ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images";
    config.autoUpdateElement = true;
};

CKEDITOR.stylesSet.add('default', [
    // Block Styles
    // { name: '標題 - 樣式1', element: 'h1', attributes: { 'class': 'colored' } },
    // { name: '標題 - 樣式2', element: 'h2', attributes: { 'class': 'colored' } },
    { name: '標題 - 樣式1', element: 'h3', attributes: { 'class': 'colored' } },
    { name: '標題 - 樣式2', element: 'h4', attributes: { 'class': 'colored' } },
    { name: '標題 - 樣式3', element: 'h5', attributes: { 'class': 'colored' } },
    { name: '標題 - 樣式4', element: 'h6', attributes: { 'class': 'colored' } },
    { name: '段落 - 引言', element: 'p', attributes: { 'class': 'leading' } },

    // Inline Styles
    { name: '文字 - 強調1', element: 'strong', attributes: { 'class': 'strong2' } },
    { name: '文字 - 強調2', element: 'strong', attributes: { 'class': 'strong3' } },
    { name: '文字 - 裝飾1', element: 'span', attributes: { 'class': 'underline' } },
    { name: '文字 - 裝飾2', element: 'span', attributes: { 'class': 'arrow-right' } },

    // Object Styles
    { name: '列表 - 樣式1', element: 'ul', attributes: { 'class': 'list-icon list-underline' } },
    { name: '列表 - 樣式2', element: 'ul', attributes: { 'class': 'list-icon' } },
    { name: '列表 - 樣式3', element: 'ul', attributes: { 'class': 'list-no-icon list-underline' } },
    { name: '數字列表 - 樣式1', element: 'ol', attributes: { 'class': 'list-styled list-underline' } },
    { name: '數字列表 - 樣式2', element: 'ol', attributes: { 'class': 'list-underline' } },
    { name: '數字列表 - 樣式3', element: 'ol', attributes: { 'class': 'list-styled' } },
    { name: '表格 - 樣式1', element: 'table', attributes: { 'class': 'table' } },
    { name: '圖片 - 加框', element: 'img', attributes: { 'class': 'thumb' } }
]);