var GridRow = React.createClass({
	mixins: [React.addons.LinkedStateMixin], 
	getInitialState: function() {  
		return { 
		};  
	},
	delCheck:function(i,chd){
		this.props.delCheck(i,chd);
	},
	modify:function(){
		this.props.updateType(this.props.primKey);
	},
	render:function(){
		return (

				<tr>
					<td className="text-center"><GridCheckDel iKey={this.props.ikey} chd={this.props.itemData.check_del} delCheck={this.delCheck} /></td>
					<td className="text-center"><GridButtonModify modify={this.modify}/></td>

					<td>{this.props.itemData.info_title}</td>
					<td>{this.props.itemData.memo}</td>
				</tr>
			);
		}
});

//主表單
var GirdForm = React.createClass({
	mixins: [React.addons.LinkedStateMixin], 
	getInitialState: function() {  
		return {
			gridData:{rows:[],page:1},
			fieldData:{info_id:gb_info_id},
			searchData:{i_Lang:'zh-TW'},
			edit_type:0,
			checkAll:false
		};  
	},
	getDefaultProps:function(){
		return{	
			fdName:'fieldData',
			gdName:'searchData',
			apiPathName:gb_approot+'api/Info'
		};
	},	
	componentWillMount:function(){
		//在輸出前觸發，只執行一次如果您在這個方法中呼叫 setState() ，會發現雖然 render() 再次被觸發了但它還是只執行一次。
	},
	componentDidMount:function(){
		//只在客戶端執行一次，當渲染完成後立即執行。當生命週期執行到這一步，元件已經俱有 DOM 所以我們可以透過 this.getDOMNode() 來取得 DOM 。
		//如果您想整和其他 Javascript framework ，使用 setTimeout, setInterval, 或者是發動 AJAX 請在這個方法中執行這些動作。
		//this.queryGridData(1);
	},
	componentWillReceiveProps:function(nextProps){
		//當元件收到新的 props 時被執行，這個方法在初始化時並不會被執行。使用的時機是在我們使用 setState() 並且呼叫 render() 之前您可以比對 props，舊的值在 this.props，而新值就從 nextProps 來。
	},
	shouldComponentUpdate:function(nextProps,nextState){
		/*
		如同其命名，是用來判斷元件是否該更新，當 props 或者 state 變更時會再重新 render 之前被執行。這個方法在初始化時不會被執行，或者當您使用了 forceUpdate 也不會被執行。
		當你確定改變的 props 或 state 並不需要觸發元件更新時，在這個方法中適當的回傳 false 可以提升一些效能。

		shouldComponentUpdate: function(nextProps, nextState) {
  			return nextProps.id !== this.props.id;
		}

		如果 shouldComponentUpdate 回傳 false 則 render() 就會完全被跳過直到下一次 state 改變，此外 componentWillUpdate 和 componentDidUpdate 將不會被觸發。
		當 state 產生異動，為了防止一些奇妙的 bug 產生，預設 shouldComponentUpdate 永遠回傳 true ，不過如果您總是使用不可變性(immutable)的方式來使用 state，並且只在 render 讀取它們那麼你可以複寫 shouldComponentUpdate
		或者是當效能遇到瓶頸，特別是需要處理大量元件時，使用 shouldComponentUpdate 通常能有效地提升速度。
		*/
		return true;
	},
	componentWillUpdate:function(nextProps,nextState){
		/*
			當收到 props 或者 state 立即執行，這個方法在初始化時不會被執行，使用時機通常是在準備更新之前。
			注意您不能在這個方法中使用 this.setState()。如果您需要在修改 props 之後更新 state 請使用 componentWillReceiveProps 取代
		*/
	},
	componentDidUpdate:function(prevProps, prevState){
		/*
			在元件更新之後執行。這個方法同樣不在初始化時執行，使用時機為當元件被更新之後需要執行一些操作。
		*/
	},
	componentWillUnmount:function(){
		//元件被從 DOM 卸載之前執行，通常我們在這個方法清除一些不再需要地物件或 timer。
	},
	handleSubmit: function(detailData,e) {

		e.preventDefault();

		this.state.fieldData.InfoDetail = detailData;
			jqPut(this.props.apiPathName,this.state.fieldData)
			.done(function(data, textStatus, jqXHRdata) {
				if(data.result){
					this.refs.subGridForm.queryGridData(this.state.searchData.i_Lang);
					tosMessage(null,'修改完成',1);
				}else{
					alert(data.message);
				}
			}.bind(this))
			.fail(function( jqXHR, textStatus, errorThrown ) {
				showAjaxError(errorThrown);
			});

		return;
	},
	changeFDValue:function(name,e){
		this.setInputValue(this.props.fdName,name,e);
	},
	changeGDValue:function(name,e){
		this.setInputValue(this.props.gdName,name,e);
	},
	setInputValue:function(collentName,name,e){

		var obj = this.state[collentName];
		if(e.target.value=='true'){
			obj[name] = true;
		}else if(e.target.value=='false'){
			obj[name] = false;
		}else{
			obj[name] = e.target.value;
		}
		this.setState({fieldData:obj});
	},
	changeLangVal:function(e){
		//this.state.searchData.i_Lang=e.target.value;
		this.setState({searchData:{i_Lang:e.target.value}});

		this.refs.subGridForm.queryGridData(e.target.value);
	},
	render: function() {
		var outHtml = null;

			var fieldData = this.state.fieldData;
			var searchData=this.state.searchData

			outHtml=(
			<div>
				<ul className="breadcrumb">
					<li><i className="fa-list-alt"></i> {this.props.menuName}</li>
				</ul>
				<h3 className="title clearfix">
					<span className="pull-left">{this.props.caption}</span>
					{/*<div className="form-inline pull-left col-xs-offset-1">
						<label><small>選擇語系：</small></label>
						<select className="form-control" 
							value={searchData.i_Lang}
							onChange={this.changeLangVal.bind(this)}
						>
							{
								CommData.LangData.map(function(itemData,i) {
									return <option key={itemData.val} value={itemData.val}>{itemData.label}</option>;
								})
							}
						</select>
					</div>*/}
				</h3>

				<SubGirdForm 
					MainId={fieldData.info_id} 
					handleSubmit={this.handleSubmit}
					Lang={this.state.searchData.i_Lang} 
					ref="subGridForm" />
			</div>
				);
		return outHtml;
	}
});

//明細列表
var SubGirdForm = React.createClass({
	mixins: [React.addons.LinkedStateMixin,SortableMixin], 
	getInitialState: function() {  
		return {
			gridData:[],
			refreshFileList:false,
			newId:0
		};  
	},
	getDefaultProps:function(){
		return{	
			fdName:'fieldData',
			gdName:'searchData',
			apiSubPathName:gb_approot+'api/InfoDetail',
			getIdPathName:gb_approot+'Active/InfoData/aj_GetNewID'
		};
	},
	componentDidMount:function(){
		this.queryGridData(this.props.Lang);
		//Sortable.create(simpleList, { /* options */ });
	},
	handleSubmit: function(e) {
		e.preventDefault();
		this.props.handleSubmit(this.state.gridData,e);
		return;
	},
	deleteSubmit:function(e){

		if(!confirm('確定是否刪除?')){
			return;
		}

		var ids = [];
		for(var i in this.state.gridData){
			if(this.state.gridData.rows[i].check_del){
				ids.push('ids='+this.state.gridData[i].info_detail_id);
			}
		}

		if(ids.length==0){
			tosMessage(null,'未選擇刪除項',2);
			return;
		}

		jqDelete(this.props.apiSubPathName+'?' + ids.join('&'),{})			
		.done(function(data, textStatus, jqXHRdata) {
			if(data.result){
				tosMessage(null,'刪除完成',1);
				this.queryGridData(0);
			}else{
				alert(data.message);
			}
		}.bind(this))
		.fail(function( jqXHR, textStatus, errorThrown ) {
			showAjaxError(errorThrown);
		});
	},
	handleSearch:function(e){
		e.preventDefault();
		this.queryGridData(0);
		return;
	},
	gridData:function(lang){
		var parms = {
			main_id:this.props.MainId,
			i_Lang:lang
		};

		return jqGet(this.props.apiSubPathName,parms);
	},
	queryGridData:function(lang){
		this.gridData(lang)
		.done(function(data, textStatus, jqXHRdata) {
			this.setState({gridData:data});
		}.bind(this))
		.fail(function(jqXHR, textStatus, errorThrown) {
			showAjaxError(errorThrown);
		});
	},
	changeFDValue:function(name,e){
		this.setInputValue(this.props.fdName,name,e);
	},
	changeGDValue:function(name,e){
		this.setInputValue(this.props.gdName,name,e);
	},
	setInputValue:function(collentName,name,e){

		var obj = this.state[collentName];
		if(e.target.value=='true'){
			obj[name] = true;
		}else if(e.target.value=='false'){
			obj[name] = false;
		}else{
			obj[name] = e.target.value;
		}
		this.setState({fieldData:obj});
	},
	setSubInputValue:function(i,name,e){
		var obj = this.state.gridData[i];
		if(e.target.value=='true'){
			obj[name] = true;
		}else if(e.target.value=='false'){
			obj[name] = false;
		}else{
			obj[name] = e.target.value;
		}
		this.setState({fieldData:obj});
	},
	creatNewData:function(type,e){
		var newState = this.state;
		jqGet(this.props.getIdPathName,{})
		.done(function(data, textStatus, jqXHRdata) {
			newState.newId=data;
			var newData = {
				info_detail_id:this.state.newId,
				info_id:this.props.MainId,
				detail_title:null,
				i_Hide:false,
				sort:0,
				stereotype:type,
				i_Lang:this.props.Lang,
				edit_state:0
			};
			newState.gridData.push(newData);
			this.setState(newState);
		}.bind(this));
	},
	deleteItem:function(i){
		var newState = this.state;
		var data = newState.gridData[i];

		if(data.edit_state==0){
			newState.gridData.splice(i,1);
			this.setState(newState);
		}else{
			jqDelete(this.props.apiSubPathName+'?ids=' + data.info_detail_id,{})
			.done(function(data, textStatus, jqXHRdata) {
				if(data.result){
					newState.gridData.splice(i,1);
					this.setState(newState);
				}else{
					tosMessage(null,data.message,1);
				}
			}.bind(this))
			.fail(function(jqXHR, textStatus, errorThrown) {
				showAjaxError(errorThrown);
			});
		}
	},
	handleSort: function (evt) { 
		var newState = this.state;
		var n = newState.gridData.length;
		for (var i in newState.gridData) {
			newState.gridData[i].sort=i;//排序改成數字越小越前面
			//n--;
		}
		newState.refreshFileList = true;
		this.setState(newState);
		this.setState({refreshFileList:false});
		//this.reloadFileList();
	},
	sortableOptions: {
        ref: "SortForm",
        model:'gridData',
        group: "shared",
        handle: ".fa-bars",
        ghostClass: "ghost"
    },
    reloadFileList:function	(){
		//console.log(this.refs.reloadFileList.length);
		//this.refs.reloadFileList.reloadFileList();
	},
	render: function() {
		var outHtml = null;
		var fieldData = this.state.fieldData;

		outHtml=
		(
			<div>
				<ul className="list-inline row">
					<li className="col-xs-4">
						<button className="btn-lg thumbnail" type="button" onClick={this.creatNewData.bind(this,1)}>
							<span className="caption"><i className="fa-plus-circle"></i> 增加版型 1 欄位</span>
							<img src="../../_Code/Images/editor_layout1.gif" />
						</button>
					</li>
					<li className="col-xs-4">
						<button className="btn-lg thumbnail" type="button" onClick={this.creatNewData.bind(this,2)}>
							<span className="caption"><i className="fa-plus-circle"></i> 增加版型 2 欄位</span>
							<img src="../../_Code/Images/editor_layout2.gif" />
						</button>
					</li>
					<li className="col-xs-4">
						<button className="btn-lg thumbnail" type="button" onClick={this.creatNewData.bind(this,3)}>
							<span className="caption"><i className="fa-plus-circle"></i> 增加版型 3 欄位</span>
							<img src="../../_Code/Images/editor_layout3.gif" />
						</button>
					</li>
				</ul>
				<div className="alert alert-warning">
					<button type="button" className="close" data-dismiss="alert"><span aria-hidden="true">×</span></button>
					<ol>
						<li>點選 <strong className="fa-bars"></strong> 並<strong>拖曳</strong>，可修改排列順序。</li>
						<li>點選 <strong className="fa-chevron-up"></strong> 或 <strong className="fa-chevron-down"></strong> 可收合/展開，點選 <strong className="fa-times"></strong> 可刪除。</li>
					</ol>
				</div>
				<form className="form-horizontal" onSubmit={this.handleSubmit}>
					<div className="panel-group" ref="SortForm">
					{
						this.state.gridData.map(function(itemData,i) {
							return <SubGirdField key={itemData.info_detail_id} ikey={i} fieldData={itemData} 
							SetSubInputValue={this.setSubInputValue} 
							DeleteItem={this.deleteItem}
							refreshFileList={this.state.refreshFileList}
							/>;
						}.bind(this))
					}
					</div>
					<div className="form-action text-center">
						<button type="submit" className="btn-primary"><i className="fa-check"></i> 儲存</button>
					</div>
				</form>
			</div>
		);
		return outHtml;
	}
});

//明細表單
var SubGirdField = React.createClass({
	mixins: [React.addons.LinkedStateMixin], 
	getInitialState: function() {  
		return {
			fieldData:this.props.fieldData,
			editorName:'content-'+this.props.ikey,
			editorObj:null			
		};  
	},
	getDefaultProps:function(){
		return{	
			subHtml:null

		};
	},
	componentDidMount:function(){
		this.setEditor(this.state.editorName);//設定及啟動editor
		
		if(this.state.fieldData.edit_state==0){
			this.state.fieldData.sort=this.props.ikey;//排序預設跟資料順序一樣
		}
	},
	componentWillReceiveProps:function(nextProps){
		this.state.fieldData = nextProps.fieldData;
		//

	},
	componentDidUpdate:function(prevProps, prevState){
		if(prevProps.fieldData.sort!=this.props.fieldData.sort){
			console.log('change sort');
			//sortable 時 編輯器的值需要自己去改變，要不然不會轉換
			this.state.editorObj.setData(this.props.fieldData.detail_content);
		}

	},
	changeFDValue:function(name,e){
		this.props.SetSubInputValue(this.props.ikey,name,e);
	},
	deleteItem:function(i){
		if(this.props.fieldData.edit_state==1){
			if(confirm('此筆資料已存在，確認是否刪除?')){
				this.props.DeleteItem(i);
			}
		}else{
			this.props.DeleteItem(i);
		}
	},
	setEditor:function(editorName){
		CKEDITOR.disableAutoInline = true;
        var cfg2={customConfig: '../ckeditor/inlineConfig.js'};//自訂額外config
		this.state.editorObj=CKEDITOR.inline(editorName,cfg2);
				
		//ckeditor change value
		this.state.editorObj.on( 'change', function( evt ) {
			this.state.fieldData.detail_content=this.state.editorObj.getData();
		}.bind(this));
	},
	reloadFileList:function	(){
		this.refs.reloadFileList.createFileUpLoadObject();
		this.refs.reloadFileList.getFileList();
	},
	render: function() {
		var outHtml = null;
		var fieldData = this.state.fieldData;

		if(fieldData.stereotype==1){
			this.props.subHtml=(
			<div id={'item-' + this.props.ikey} className="panel-collapse collapse in">
				<div className="panel-body">
					<div className="editor">
						<textarea className="form-control" rows="4" id={this.state.editorName}
						contenteditable="true"
						value={fieldData.detail_content}
						onChange={this.changeFDValue.bind(this,'detail_content')} />
					</div>
				</div>
			</div>);
		}else if(fieldData.stereotype==2){
			this.props.subHtml=(
			<div id={'item-' + this.props.ikey} className="panel-collapse collapse in">
				<div className="panel-body">
					<div className="form-group">
						<div className="col-xs-8">
							<div className="editor">
								<textarea className="form-control" rows="4" id={this.state.editorName}
								contenteditable="true"
								value={fieldData.detail_content}
								onChange={this.changeFDValue.bind(this,'detail_content')} />
							</div>
						</div>
						<div className="col-xs-4">
							<InputFileUploadforEdirtor ikey={this.props.ikey} 
							MainId={fieldData.info_detail_id} 
							FileKind="Photo1" 
							edit_state={fieldData.edit_state}
							ref="reloadFileList"
							refreshFileList={this.props.refreshFileList}
							collapseId={'item-' + this.props.ikey}
							url_upload={gb_approot + 'Active/InfoData/axFUpload'}
							url_list={gb_approot+'Active/InfoData/axFList'}
							url_delete={gb_approot+'Active/InfoData/axFDelete'}
							url_sort={gb_approot+'Active/InfoData/axFSort'}
							/>
						</div>
					</div>
				</div>
			</div>
				);
		}else if(fieldData.stereotype==3){
			this.props.subHtml=(
			<div id={'item-' + this.props.ikey} className="panel-collapse collapse in">
				<div className="panel-body">
					<div className="form-group">
						<div className="col-xs-4">
							<InputFileUploadforEdirtor ikey={this.props.ikey} 
							MainId={fieldData.info_detail_id} 
							FileKind="Photo2" 
							edit_state={fieldData.edit_state}
							ref="reloadFileList"
							refreshFileList={this.props.refreshFileList}
							url_upload={gb_approot + 'Active/InfoData/axFUpload'}
							url_list={gb_approot+'Active/InfoData/axFList'}
							url_delete={gb_approot+'Active/InfoData/axFDelete'}
							url_sort={gb_approot+'Active/InfoData/axFSort'}
							/>
						</div>
						<div className="col-xs-8">
							<div className="editor">
								<textarea className="form-control" rows="4" id={this.state.editorName}
								contenteditable="true"
								value={fieldData.detail_content}
								onChange={this.changeFDValue.bind(this,'detail_content')} />
							</div>
						</div>
					</div>
				</div>
			</div>
				);
		}
		outHtml = (
		<div className="panel">
			<div className="panel-heading">
				<h4 className="panel-title draggable clearfix">
					<i className="fa-bars"></i>
					<div className="form-horizontal">
						<div className="col-xs-10">
							<div className="input-group">
								<span className="input-group-addon">#{this.props.ikey}</span>
								<input type="text" className="form-control"
								value={fieldData.detail_title}
								onChange={this.changeFDValue.bind(this,'detail_title')}
								maxLength="60"
								placeholder="請在此輸入標題"
								required />
							</div>
						</div>
						<div className="col-xs-1">
							<small class="help-inline">最多60字</small>
						</div>
					</div>
					<ul className="widget">
						<li><a data-toggle="collapse" href={'#item-' + this.props.ikey} title="收合/展開" className="text-default"><i className="fa-chevron-down"></i></a></li>
						<li><button className="btn-link text-danger" title="刪除" onClick={this.deleteItem.bind(this,this.props.ikey)}><i className="fa-times"></i></button></li>
					</ul>
				</h4>
			</div>
			{this.props.subHtml}
		</div>
		);
		return outHtml;
	}
});

var InputFileUploadforEdirtor = React.createClass({
	mixins: [React.addons.LinkedStateMixin], 
	getInitialState: function() {  
		return {
			filelist:[]
		};  
	},
	getDefaultProps:function(){
		return{	
			ikey:0,
			MainId:0,
			edit_state:0,
			uploader:null
		};
	},
	componentDidUpdate:function(prevProps, prevState){
		if( (prevProps.edit_state==0 && this.props.edit_state==1) || this.props.refreshFileList) {
			this.createFileUpLoadObject();
			this.getFileList();
		}
	},
	componentDidMount:function(){
		if(this.props.edit_state==1){
			this.createFileUpLoadObject();
			this.getFileList();
		}
		$('#item-' + this.props.ikey).on('hidden.bs.collapse', function () {
  			// 隱藏卷軸時將圖片上傳的按鈕給destroy
  			if(this.props.uploader!=null){
			this.props.uploader.destroy();
			}
		}.bind(this));
		$('#item-' + this.props.ikey).on('shown.bs.collapse', function () {
  			// 顯示卷軸時將圖片上傳的按鈕給重新啟動功能
  			this.createFileUpLoadObject();
			this.getFileList();
		}.bind(this));
	},
	componentWillReceiveProps:function(nextProps){
		
	},
	componentWillUnmount:function(){
		console.log('MasterFileUpload','destroy');
		if(this.props.uploader!=null){
			this.props.uploader.destroy();
		}
	},
	shouldComponentUpdate:function(nextProps,nextState){
		// return (this.props.edit_state == 0 && nextProps.edit_state==1);
		return true;
	},
	changeFDValue:function(name,e){
		this.props.SetSubInputValue(this.props.ikey,name,e);
	},
	deleteFile:function(filename){
		jqPost(this.props.url_delete,{
			id:this.props.MainId,
			fileKind:this.props.FileKind,
			filename:filename
		})			
		.done(function(data, textStatus, jqXHRdata) {
			if(data.result){
				this.getFileList();
			}else{
				alert(data.message);
			}
		}.bind(this))
		.fail(function( jqXHR, textStatus, errorThrown ) {
			showAjaxError(errorThrown);
		});
	},
	getFileList:function(){
		jqPost(this.props.url_list,{
			id:this.props.MainId,
			fileKind:this.props.FileKind
		})			
		.done(function(data, textStatus, jqXHRdata) {
			if(data.result){
				this.setState({filelist:data.files})
			}else{
				alert(data.message);
			}
		}.bind(this))
		.fail(function( jqXHR, textStatus, errorThrown ) {
			showAjaxError(errorThrown);
		});
	},
	createFileUpLoadObject:function(){
			var btn = document.getElementById('upload-btn-' + this.props.ikey);
			var r_this = this;
		  	this.props.uploader  = new ss.SimpleUpload({
		        button: btn,
		        url: this.props.url_upload,
		        data:{
		        	id:this.props.MainId,
		        	fileKind:this.props.FileKind
		        },
		        name: 'fileName',
		        multiple: true,
		        maxSize: 5000,
		        allowedExtensions: ['jpg', 'jpeg', 'png', 'gif'],
		        accept: 'image/*',
		        responseType: 'json',
				onSubmit: function(filename, ext) {            
					if(r_this.props.MainId==0){
						alert('此筆資料未完成新增，無法上傳檔案!')
						return false;
					}
				},
				onProgress:function(pct){
					console.log('Progress',pct);
				},		
				onSizeError: function() {
					alert('Files may not exceed 500K.');
		                //errBox.innerHTML = 'Files may not exceed 500K.';
				},
				onExtError: function() {
					alert('Invalid file type. Please select a PNG, JPG, GIF image.');
		              //errBox.innerHTML = 'Invalid file type. Please select a PNG, JPG, GIF image.';
				},
		        onComplete: function(file, response) {
		        	if(response.result){ 
						r_this.getFileList();
					}else{

					}
		        }
			});
	},
	render: function() {
		var imgHtml=null;
		if(this.props.MainId!=0){
			imgHtml=(
				<div className="row">
					<div className="col-xs-10">
						<div className="form-control">
							<input type="file" id={'upload-btn-' + this.props.ikey} />
						</div>
					</div>
				</div>);
		}else{
			imgHtml=(
				<div className="form-control">
				<small className="col-xs-6 help-inline">請先按儲存後方可上傳圖片</small>
				</div>);
		}
		var outHtml = null;
		var classVal="img-upload";
		if(this.props.FileKind=="Photo1"){
			classVal="float-r";
		}else if(this.props.FileKind=="Photo2"){
			classVal="float-l";			
		}
		outHtml=(				
		<div>
			{imgHtml}
			<p className="help-block">
			{
				this.state.filelist.map(function(itemData,i) {
					var  subOutHtml =
					<span className={classVal} key={i}>
						<button type="button" className="close" onClick={this.deleteFile.bind(this,itemData.fileName)}>&times;</button>
						<img src={itemData.originPath} />
					</span>;
					return subOutHtml;
				},this)
			}
			</p>
		</div>
		);
		return outHtml;
	}
});