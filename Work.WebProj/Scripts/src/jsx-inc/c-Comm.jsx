var GridButtonModify = React.createClass({
	getInitialState: function() {  
		return { 
			value:'c'
		};  
	},
	onClick:function(e){
		this.props.modify();
	},
	render:function(){
		return (
			<button type="button" className="btn-link btn-lg" onClick={this.onClick}><i className="fa-pencil"></i></button>
			);
	}
});
var GridButtonView = React.createClass({
	getInitialState: function() {  
		return { 
		};  
	},
	onClick:function(e){
		this.props.modify();
	},
	render:function(){
		return (
			<button type="button" className="btn-link btn-lg" onClick={this.onClick}><i className="fa-search-plus"></i></button>
			);
	}
});
//popup window 修改按鈕
var GridButtonPopupWindow = React.createClass({
	getInitialState: function() {  
		return { 
		};  
	},
	onClick:function(e){
		this.props.modify();
	},
	render:function(){
		return (
			<button type="button" className="btn-link btn-lg" data-toggle="modal" data-target={'#myModal-'+this.props.MainId} onClick={this.onClick}><i className="fa-pencil"></i></button>
			);
	}
});
//縮放子類別按鈕
var GridButtonSub = React.createClass({
	getInitialState: function() {  
		return { 
			subHtml:'fa-plus'
		};  
	},
	componentWillReceiveProps:function(nextProps){
		//當元件收到新的 props 時被執行，這個方法在初始化時並不會被執行。使用的時機是在我們使用 setState() 並且呼叫 render() 之前您可以比對 props，舊的值在 this.props，而新值就從 nextProps 來。
		if(nextProps.chd){
			this.setState({subHtml:'fa-minus'});//展開
		}else{
			this.setState({subHtml:'fa-plus'});//合起
		}
	},
	onClick:function(e){
		this.props.subCheck(this.props.iKey,this.props.chd);
		this.props.chd=!this.props.chd;
		if(this.props.chd){
			this.setState({subHtml:'fa-minus'});//展開
		}else{
			this.setState({subHtml:'fa-plus'});//合起
		}
	},
	onChange:function(e){
		this.props.subCheck(this.props.iKey,this.props.chd);
		this.props.chd=!this.props.chd;
		if(this.props.chd){
			this.setState({subHtml:'fa-minus'});//展開
		}else{
			this.setState({subHtml:'fa-plus'});//合起
		}
	},
	render:function(){
		return (
			<button type="button" className="btn-link btn-lg" onClick={this.onClick}><i className={this.state.subHtml}></i></button>
			);
	}
});

var GridCheckDel = React.createClass({
	getInitialState: function() {  
		return { 
		};  
	},
	onChange:function(e){
		this.props.delCheck(this.props.iKey,this.props.chd);
	},
	render:function(){
		return (
			<label className="cbox">
				<input type="checkbox" checked={this.props.chd} onChange={this.onChange} />
				<i className="fa-check"></i>
			</label>
			);
	}
});

var GridNavPage = React.createClass({
	getInitialState: function() {  
		return {
		};  
	},
	getDefaultProps:function(){
		return{
			gridData:null,
			onQueryGridData:null,
			InsertType:0,
			UpdateType:null,
			deleteSubmit:null,
			showAdd:true,
			showDelete:true
		};
	},
	firstPage:function(){
		this.props.onQueryGridData(1);
	},
	lastPage:function(){
		this.props.onQueryGridData(this.props.TotalPage);
	},
	nextPage:function(){
		if(this.props.NowPage < this.props.TotalPage){
			this.props.onQueryGridData(this.props.NowPage + 1);
		}
	},
	prvePage:function(){
		if(this.props.NowPage > 1){
			this.props.onQueryGridData(this.props.NowPage - 1);
		}
	},
	jumpPage:function(){

	},
	render:function(){

		var setAddButton = null,setDeleteButton=null;
		if(this.props.showAdd){
			setAddButton = <button className="btn-link text-success"
			                type="button"
			                onClick={this.props.InsertType}>
			            	<i className="fa-plus-circle"></i> 新增
			        		</button>;			        		
		}

		if(this.props.showDelete){
			setDeleteButton = 	<button className="btn-link text-danger" type="button"
			                		onClick={this.props.deleteSubmit}>
			            			<i className="fa-trash-o"></i> 刪除
			        			</button>;

		}
		var oper = null;

		oper = (
			<div className="table-footer">
			    <div className="pull-left">
			        {setAddButton}
			        {setDeleteButton}
			    </div>
			    <small className="pull-right">第{this.props.StartCount}-{this.props.EndCount}筆，共{this.props.RecordCount}筆</small>

			    <ul className="pager">
			        <li>
			            <a href="#" title="移至第一頁" tabIndex="-1" onClick={this.firstPage}>
			                <i className="fa-angle-double-left"></i>
			            </a>
			        </li> { } 
			        <li>
			            <a href="#" title="上一頁" tabIndex="-1" onClick={this.prvePage}>
			                <i className="fa-angle-left"></i>
			            </a>
			        </li> { } 
			        <li className="form-inline">
			            <div className="form-group">
			                <label>第</label>
			                {' '}
			                <input className="form-control" type="number" min="1" tabIndex="-1" value={this.props.NowPage}
			                       onChange={this.jumpPage} />
			                {' '}
			                <label>頁，共{this.props.TotalPage}頁</label>
			            </div>
			        </li> { } 
			        <li>
			            <a href="#" title="@Resources.Res.NextPage" tabIndex="-1" onClick={this.nextPage}>
			                <i className="fa-angle-right"></i>
			            </a>
			        </li> { } 
			        <li>
			            <a href="#" title="移至最後一頁" tabIndex="-1" onClick={this.lastPage}>
			                <i className="fa-angle-double-right"></i>
			            </a>
			        </li>
			    </ul>
			</div>
		);

		return oper;
	}
});
//新增按鈕為popup window
var GridNavPageUsePopup = React.createClass({
	getInitialState: function() {  
		return {
		};  
	},
	getDefaultProps:function(){
		return{
			gridData:null,
			onQueryGridData:null,
			InsertType:0,
			UpdateType:null,
			deleteSubmit:null,
			showAdd:true,
			showDelete:true
		};
	},
	firstPage:function(){
		this.props.onQueryGridData(1);
	},
	lastPage:function(){
		this.props.onQueryGridData(this.props.TotalPage);
	},
	nextPage:function(){
		if(this.props.NowPage < this.props.TotalPage){
			this.props.onQueryGridData(this.props.NowPage + 1);
		}
	},
	prvePage:function(){
		if(this.props.NowPage > 1){
			this.props.onQueryGridData(this.props.NowPage - 1);
		}
	},
	jumpPage:function(){

	},
	render:function(){

		var setAddButton = null,setDeleteButton=null;
		if(this.props.showAdd){
			setAddButton = <button className="btn-link text-success"
			                type="button"
			                data-toggle="modal"
			                data-target={'#myModal-'+this.props.MainId}
			                onClick={this.props.InsertType}>
			            	<i className="fa-plus-circle"></i> 新增
			        		</button>;			        		
		}

		if(this.props.showDelete){
			setDeleteButton = 	<button className="btn-link text-danger" type="button"
			                		onClick={this.props.deleteSubmit}>
			            			<i className="fa-trash-o"></i> 刪除
			        			</button>;

		}
		var oper = null;

		oper = (
			<div className="table-footer">
			    <div className="pull-left">
			        {setAddButton}
			        {setDeleteButton}
			    </div>
			    <small className="pull-right">第{this.props.StartCount}-{this.props.EndCount}筆，共{this.props.RecordCount}筆</small>

			    <ul className="pager">
			        <li>
			            <a href="#" title="移至第一頁" tabIndex="-1" onClick={this.firstPage}>
			                <i className="fa-angle-double-left"></i>
			            </a>
			        </li> { } 
			        <li>
			            <a href="#" title="上一頁" tabIndex="-1" onClick={this.prvePage}>
			                <i className="fa-angle-left"></i>
			            </a>
			        </li> { } 
			        <li className="form-inline">
			            <div className="form-group">
			                <label>第</label>
			                {' '}
			                <input className="form-control" type="number" min="1" tabIndex="-1" value={this.props.NowPage}
			                       onChange={this.jumpPage} />
			                {' '}
			                <label>頁，共{this.props.TotalPage}頁</label>
			            </div>
			        </li> { } 
			        <li>
			            <a href="#" title="@Resources.Res.NextPage" tabIndex="-1" onClick={this.nextPage}>
			                <i className="fa-angle-right"></i>
			            </a>
			        </li> { } 
			        <li>
			            <a href="#" title="移至最後一頁" tabIndex="-1" onClick={this.lastPage}>
			                <i className="fa-angle-double-right"></i>
			            </a>
			        </li>
			    </ul>
			</div>
		);

		return oper;
	}
});


//日期輸入元件
var InputDate = React.createClass({
	mixins: [React.addons.LinkedStateMixin], 
	getInitialState: function() {  
		return { 
		};  
	},
	getDefaultProps:function(){
		return{	
			value:null,
			onChange:null,
			field_name:null,
			required:false
		};
	},
	componentDidMount:function(){
		$('#' + this.props.id).datetimepicker(
			{
				format:'YYYY-MM-DD',
				icons: {
					previous: "fa-angle-left",
	                next: "fa-angle-right"
				}
			}).on('dp.change',function(e){
				this.props.onChange(this.props.field_name,e);
			}.bind(this));
	},
	componentDidUpdate:function(prevProps, prevState){
	},
	componentWillUnmount:function(){
	},
	onChange:function(e){
		this.props.onChange(this.props.field_name,e);
	},
	render:function(){

		return (
			<div>
				<input 
					type="date" 
					className="form-control datetimepicker"
					id={this.props.id}
					name={this.props.field_name}
					value={this.props.value!=undefined ? moment(this.props.value).format('YYYY-MM-DD'):''}
					onChange={this.onChange}
					required={this.props.required} />
					<i className="fa-calendar form-control-feedback"></i>
			</div>
			);
		}
});

//Image共用元件 前台用
var ImgList = React.createClass({
	getInitialState: function() {  
		return { 
		};  
	},
	getDefaultProps:function(){
		//預設值
		return{	
			SetClass:null,
			NoImagePath:gb_approot + 'Content/images/Activities/no_pic.jpg'
		};
	},
	render:function(){	
		
		if(this.props.imgsrc!=undefined){
			return <img src={this.props.imgsrc} className={this.props.SetClass} />;
		}else{
			return <img src={this.props.NoImagePath} className={this.props.SetClass} />;
		}
	}
});

//後端文件上下傳
var MasterDocFileUpload = React.createClass({
	getInitialState: function() {  
		return {
			filelist:[],
			download_src:null
		};  
	},
	getDefaultProps:function(){
		return{	
			url_upload:null,
			url_list:null,
			url_delete:null,
			url_download:null,
			FileKind:null,
			MainId:0
		};
	},
	componentDidUpdate:function(prevProps, prevState){
		//this.getFileList();
	},
	componentDidMount:function(){
		if(this.props.MainId>1){
			this.createFileUpLoadObject();
			this.getFileList();
		}
	},
	componentWillReceiveProps:function(nextProps){
		
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
				this.setState({filelist:data.filesObject})
			}else{
				alert(data.message);
			}
		}.bind(this))
		.fail(function( jqXHR, textStatus, errorThrown ) {
			showAjaxError(errorThrown);
		});
	},
	downloadFile:function(id,filekind,filename){
		var parms = [];
		parms.push('id=' + id);
		parms.push('filekind=' + filekind);
		parms.push('filename=' + filename);
		parms.push('tid=' + uniqid());
		var src = this.props.url_download + '?' + parms.join('&');
		this.setState({download_src:src});
	},
	createFileUpLoadObject:function(){
			var btn = document.getElementById('upload-btn-' + this.props.MainId);
			var r_this = this;
		  	var uploader = new ss.SimpleUpload({
		        button: btn,
		        url: this.props.url_upload,
		        data:{
		        	id:this.props.MainId,
		        	fileKind:this.props.FileKind
		        },
		        name: 'fileName',
		        multiple: true,
		        maxSize: 5000,
		        allowedExtensions: ['pdf', 'doc', 'docx','xls','xlsx','txt','png'],
		        responseType: 'json',
				onSubmit: function(filename, ext) {            
					if(r_this.props.MainId==0){
						alert('此筆資料未完成新增，無法上傳檔案!')
						return false;
					}

					btn.value = ''; 

				},
				onProgress:function(pct){
					console.log('Progress',pct);
				},		
				onSizeError: function() {
		                errBox.innerHTML = 'Files may not exceed 500K.';
				},
				onExtError: function() {
		              errBox.innerHTML = 'Invalid file type. Please select a PNG, JPG, GIF image.';
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

		var outHtml = null;

		outHtml=(				
		<div>
			<div className="form-control">
				<input type="file" id={'upload-btn-' + this.props.MainId} />
			</div>
			<p className="help-block">
			{
				this.state.filelist.map(function(itemData,i) {
					var  subOutHtml =
					<span className="doc-upload" key={i}>
						<i className="fa-file-text-o"></i>
						<button type="button" className="close" onClick={this.deleteFile.bind(this,itemData.FileName)}>&times;</button>
						<button type="button" className="btn-link" onClick={this.downloadFile.bind(this,this.props.MainId,this.props.FileKind,itemData.FileName)}>
						{itemData.FileName}
						</button>
					</span>;
					return subOutHtml;
				},this)
			}
			</p>
			<iframe src={this.state.download_src} style={ {visibility:'hidden',display:'none'} } />
		</div>
		);
		return outHtml;
	}
});

var MasterDocFileUploadView = React.createClass({
	getInitialState: function() {  
		return {
			filelist:[],
			download_src:null
		};  
	},
	getDefaultProps:function(){
		return{	
			url_upload:null,
			url_list:null,
			url_delete:null,
			url_download:null,
			FileKind:null,
			MainId:0
		};
	},
	componentDidUpdate:function(prevProps, prevState){
		//this.getFileList();
	},
	componentDidMount:function(){
		if(this.props.MainId>1){
			this.getFileList();
		}
	},
	componentWillReceiveProps:function(nextProps){
		
	},
	changeFDValue:function(name,e){
		this.props.SetSubInputValue(this.props.ikey,name,e);
	},
	getFileList:function(){
		jqPost(this.props.url_list,{
			id:this.props.MainId,
			fileKind:this.props.FileKind
		})			
		.done(function(data, textStatus, jqXHRdata) {
			if(data.result){
				this.setState({filelist:data.filesObject})
			}else{
				alert(data.message);
			}
		}.bind(this))
		.fail(function( jqXHR, textStatus, errorThrown ) {
			showAjaxError(errorThrown);
		});
	},
	downloadFile:function(id,filekind,filename){
		var parms = [];
		parms.push('id=' + id);
		parms.push('filekind=' + filekind);
		parms.push('filename=' + filename);
		parms.push('tid=' + uniqid());
		var src = this.props.url_download + '?' + parms.join('&');
		this.setState({download_src:src});
	},
	render: function() {

		var outHtml = null;

		outHtml=(				
		<div>
			<p className="help-block">
			{
				this.state.filelist.map(function(itemData,i) {
					var  subOutHtml =
					<span className="doc-upload" key={i}>
						<i className="fa-file-text-o"></i>
						<button type="button" className="btn-link" onClick={this.downloadFile.bind(this,this.props.MainId,this.props.FileKind,itemData.FileName)}>
						{itemData.FileName}
						</button>
					</span>;
					return subOutHtml;
				},this)
			}
			</p>
			<iframe src={this.state.download_src} style={ {visibility:'hidden',display:'none'} } />
		</div>
		);
		return outHtml;
	}
});

//前台換頁
var PageWWW = React.createClass({
	getInitialState: function() {  
		return {
		};  
	},
	getDefaultProps:function(){
		return{	
			pageFirst:null,
			pagePrve:null,
			page:null,
			pageSelect:null,
			pageNext:null,
			pageLast:null,
			total:null,
			lang:0
		};
	},
	render: function() {

		var outHtml = null;
		if(this.props.lang==0){
			outHtml=(		
				<form>
					<ul className="pager">
						<li><button type="button" onClick={this.props.pageFirst} disabled={this.props.page==1}>第一頁</button></li>
						<li><button type="button" onClick={this.props.pagePrve} disabled={this.props.page==1}>上一頁</button></li>
						<li>
							<select value={this.props.page} onChange={this.props.pageSelect}>
								{this.props.dpage}
							</select>
						</li>
						<li><button type="button" onClick={this.props.pageNext} disabled={this.props.page==this.props.total}>下一頁</button></li>
						<li><button type="button" onClick={this.props.pageLast} disabled={this.props.page==this.props.total}>最末頁</button></li>
					</ul>
					<p className="pagination">第<input type="number" value={this.props.page} onChange={this.props.pageSelect} />之 <span>{this.props.total}</span>頁</p>
				</form>
			);
		}else if(this.props.lang==1){
			outHtml=(		
				<form>
					<ul className="pager">
						<li><button type="button" onClick={this.props.pageFirst} disabled={this.props.page==1}>第一页</button></li>
						<li><button type="button" onClick={this.props.pagePrve} disabled={this.props.page==1}>上一页</button></li>
						<li>
							<select value={this.props.page} onChange={this.props.pageSelect}>
								{this.props.dpage}
							</select>
						</li>
						<li><button type="button" onClick={this.props.pageNext} disabled={this.props.page==this.props.total}>下一页</button></li>
						<li><button type="button" onClick={this.props.pageLast} disabled={this.props.page==this.props.total}>最末页</button></li>
					</ul>
					<p className="pagination">第<input type="number" value={this.props.page} onChange={this.props.pageSelect} />之 <span>{this.props.total}</span>页</p>
				</form>
			);
		}

		return outHtml;
	}
});
