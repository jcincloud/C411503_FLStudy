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
			fieldData:{},
			searchData:{title:null},
			edit_type:0,
			checkAll:false
		};  
	},
	getDefaultProps:function(){
		return{	
			fdName:'fieldData',
			gdName:'searchData',
			apiPathName:gb_approot+'api/Info',
			initPathName:gb_approot+'Active/InfoData/aj_Init'

		};
	},	
	componentWillMount:function(){
		//在輸出前觸發，只執行一次如果您在這個方法中呼叫 setState() ，會發現雖然 render() 再次被觸發了但它還是只執行一次。
	},
	componentDidMount:function(){
		//只在客戶端執行一次，當渲染完成後立即執行。當生命週期執行到這一步，元件已經俱有 DOM 所以我們可以透過 this.getDOMNode() 來取得 DOM 。
		//如果您想整和其他 Javascript framework ，使用 setTimeout, setInterval, 或者是發動 AJAX 請在這個方法中執行這些動作。
		this.queryGridData(1);
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
		if(this.state.edit_type==1){
			jqPost(this.props.apiPathName,this.state.fieldData)
			.done(function(data, textStatus, jqXHRdata) {
				if(data.result){
					tosMessage(null,'新增完成',1);
					this.updateType(data.id);
				}else{
					alert(data.message);
				}
			}.bind(this))
			.fail(function( jqXHR, textStatus, errorThrown ) {
				showAjaxError(errorThrown);
			});
		}		
		else if(this.state.edit_type==2){
			jqPut(this.props.apiPathName,this.state.fieldData)
			.done(function(data, textStatus, jqXHRdata) {
				if(data.result){
					this.refs.subGridForm.queryGridData();
					tosMessage(null,'修改完成',1);
				}else{
					alert(data.message);
				}
			}.bind(this))
			.fail(function( jqXHR, textStatus, errorThrown ) {
				showAjaxError(errorThrown);
			});
		};
		return;
	},
	deleteSubmit:function(e){

		if(!confirm('確定是否刪除?')){
			return;
		}

		var ids = [];
		for(var i in this.state.gridData.rows){
			if(this.state.gridData.rows[i].check_del){
				ids.push('ids='+this.state.gridData.rows[i].info_id);
			}
		}

		if(ids.length==0){
			tosMessage(null,'未選擇刪除項',2);
			return;
		}

		jqDelete(this.props.apiPathName+'?' + ids.join('&'),{})			
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
	delCheck:function(i,chd){

		var newState = this.state;
		this.state.gridData.rows[i].check_del = !chd;
		this.setState(newState);
	},
	checkAll:function(){

		var newState = this.state;
		newState.checkAll = !newState.checkAll;
		for (var prop in this.state.gridData.rows) {
			this.state.gridData.rows[prop].check_del=newState.checkAll;
		}
		this.setState(newState);
	},
	gridData:function(page){

		var parms = {
			page:0
		};

		if(page==0){
			parms.page=this.state.gridData.page;
		}else{
			parms.page=page;
		}

		$.extend(parms, this.state.searchData);

		return jqGet(this.props.apiPathName,parms);
	},
	queryGridData:function(page){
		this.gridData(page)
		.done(function(data, textStatus, jqXHRdata) {
			this.setState({gridData:data});
		}.bind(this))
		.fail(function(jqXHR, textStatus, errorThrown) {
			showAjaxError(errorThrown);
		});
	},
	insertType:function(){
		this.setState({edit_type:1,fieldData:{}});
	},
	updateType:function(id){
		jqGet(this.props.apiPathName,{id:id})
		.done(function(data, textStatus, jqXHRdata) {
			this.setState({edit_type:2,fieldData:data.data});
		}.bind(this))
		.fail(function( jqXHR, textStatus, errorThrown ) {
			showAjaxError(errorThrown);
		});
	},
	noneType:function(){
		this.gridData(0)
		.done(function(data, textStatus, jqXHRdata) {
			this.setState({edit_type:0,gridData:data});
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
	render: function() {
		var outHtml = null;

		if(this.state.edit_type==0)
		{
			var searchData = this.state.searchData;

			outHtml =
			(
			<div>
				<ul className="breadcrumb">
					<li><i className="fa-list-alt"></i> {this.props.MenuName}</li>
				</ul>
				<h3 className="title">
					{this.props.Caption}
				</h3>
				<form onSubmit={this.handleSearch}>
					<div className="table-responsive">
						<div className="table-header">
							{/*<h5 className="table-title">搜尋<strong>這裡是關鍵字</strong>的結果:</h5>*/}
							<div className="table-filter">
								<div className="form-inline">
									<div className="form-group">
										<label>分類名稱</label> { }
										<input type="text" className="form-control input-sm" 
										value={searchData.title}
										onChange={this.changeGDValue.bind(this,'title')}
										placeholder="請輸入關鍵字..." />
										<button className="btn-primary btn-sm" type="submit"><i className="fa-search"></i> 搜尋</button>
									</div> { }
								</div>
							</div>
						</div>
						<table>
							<thead>
								<tr>
									<th className="col-xs-1 text-center">
										<label className="cbox">
											<input type="checkbox" checked={this.state.checkAll} onChange={this.checkAll} />
											<i className="fa-check"></i>
										</label>
									</th>
									<th className="col-xs-1 text-center">修改</th>
									<th className="col-xs-5">標題</th>
									<th className="col-xs-3">日期</th>
									<th className="col-xs-1">排序</th>
									<th className="col-xs-1">狀態</th>
								</tr>
							</thead>
							<tbody>
								{
								this.state.gridData.rows.map(function(itemData,i) {
								return <GridRow 
								key={i}
								ikey={i}
								primKey={itemData.info_id} 
								itemData={itemData} 
								delCheck={this.delCheck}
								updateType={this.updateType}								
								/>;
								}.bind(this))
								}
							</tbody>
						</table>
					</div>
					<GridNavPage 
					StartCount={this.state.gridData.startcount}
					EndCount={this.state.gridData.endcount}
					RecordCount={this.state.gridData.records}
					TotalPage={this.state.gridData.total}
					NowPage={this.state.gridData.page}
					onQueryGridData={this.queryGridData}
					InsertType={this.insertType}
					deleteSubmit={this.deleteSubmit}
					showAdd={false}
					showDelete={false}
					/>
				</form>
			</div>
			);
		}
		else if(this.state.edit_type==1 || this.state.edit_type==2)
		{
			var fieldData = this.state.fieldData;

			outHtml=(
			<div>
				<ul className="breadcrumb">
					<li><i className="fa-list-alt"></i> {this.props.MenuName}</li>
				</ul>
				<h4 className="title">{this.props.Caption} 基本資料維護</h4>
				<form className="form-horizontal" onSubmit={this.handleSubmit}>
					<div className="form-group">
						<label className="col-xs-1 control-label">分類名稱</label>
						<div className="col-xs-6">
							<input type="text" 
							className="form-control"	
							value={fieldData.info_title}
							onChange={this.changeFDValue.bind(this,'info_title')}
							maxLength="64"
							required />						
						</div>
					</div>

					<div className="form-group">
						<label className="col-xs-1 control-label">簡介</label>
							<div className="col-xs-6">
								<textarea type="date" className="form-control" rows="1"
									value={fieldData.memo}
									onChange={this.changeFDValue.bind(this,'memo')} />
							</div>
					</div>

					{/*<div className="form-action">
						<div className="col-xs-4 col-xs-offset-2">
							<button type="submit" className="btn-primary"><i className="fa-check"></i> 儲存</button>
							<button className="col-xs-offset-1" type="button" onClick={this.noneType}><i className="fa-times"></i> 回前頁</button>
						</div>
					</div>*/}
				</form>
				<SubGirdForm 
					MainId={fieldData.info_id} 
					handleSubmit={this.handleSubmit} 
					noneType={this.noneType} ref="subGridForm"
					ParentEditType={this.state.edit_type} />
			</div>
				);
		}else{
			outHtml=(<span>No Page</span>);
		}

		return outHtml;
	}
});

//明細列表
var SubGirdForm = React.createClass({
	mixins: [React.addons.LinkedStateMixin,SortableMixin], 
	getInitialState: function() {  
		return {
			gridData:[],
			refreshFileList:false
		};  
	},
	getDefaultProps:function(){
		return{	
			fdName:'fieldData',
			gdName:'searchData',
			apiSubPathName:gb_approot+'api/InfoDetail'
		};
	},
	componentDidMount:function(){
		this.queryGridData();
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
	gridData:function(){
		var parms = {
			main_id:this.props.MainId
		};

		return jqGet(this.props.apiSubPathName,parms);
	},
	queryGridData:function(){
		this.gridData()
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
	creatNewData:function(){

		var data = {
			info_detail_id:0,
			info_id:this.props.MainId,
			detail_title:null,
			i_Hide:false,
			sort:0,
			edit_state:0
		};
		var newState = this.state;
		newState.gridData.push(data);
		this.setState(newState);
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
			newState.gridData[i].sort=n;
			n--;
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
        handle: ".panel-title",
        ghostClass: "ghost"
    },
    reloadFileList:function	(){
		//console.log(this.refs.reloadFileList.length);
		//this.refs.reloadFileList.reloadFileList();
	},
	render: function() {
		var outHtml = null;
		var fieldData = this.state.fieldData;
		var creatNewDataHtml=null;
		if (this.props.ParentEditType==1) {

		}else if(this.props.ParentEditType==2){
			creatNewDataHtml=(
				<button className="btn-link text-success" type="button" onClick={this.creatNewData}>
						<i className="fa-plus-circle"></i> 新增子單元內容
					</button>
				);
		};
		outHtml=
		(
			<div>
				<hr className="expanded" />
				<h4 className="title">
					子單元 內容資料維護 { }
					{creatNewDataHtml}
				</h4>
				<div className="alert alert-warning">
					<button type="button" className="close" data-dismiss="alert"><span aria-hidden="true">×</span></button>
					<ol>
						<li>點選 <strong className="fa-bars"></strong> 並<strong>拖曳</strong>，可修改排列順序。</li>
						<li>點選 <strong className="fa-chevron-up"></strong> 或 <strong className="fa-chevron-down"></strong> 可收合/展開表單。</li>
					</ol>
				</div>
				<form className="form-horizontal" onSubmit={this.handleSubmit}>
					<div className="panel-group" ref="SortForm">
					{
						this.state.gridData.map(function(itemData,i) {
							return <SubGirdField key={i} ikey={i} fieldData={itemData} 
							SetSubInputValue={this.setSubInputValue} 
							DeleteItem={this.deleteItem}
							refreshFileList={this.state.refreshFileList}
							/>;
						}.bind(this))
					}
					</div>
					<div className="form-action">
						<div className="col-xs-4 col-xs-offset-2">
							<button type="submit" className="btn-primary"><i className="fa-check"></i> 儲存</button>
							<button className="col-xs-offset-1" type="button" onClick={this.props.noneType}><i className="fa-times"></i> 回列表</button>
						</div>
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
			fieldData:this.props.fieldData
		};  
	},
	getDefaultProps:function(){
		return{	
		};
	},
	componentDidMount:function(){

	},
	componentWillReceiveProps:function(nextProps){
		this.state.fieldData = nextProps.fieldData;
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
	reloadFileList:function	(){
		this.refs.reloadFileList.createFileUpLoadObject();
		this.refs.reloadFileList.getFileList();
	},
	render: function() {

		var outHtml = null;
		var fieldData = this.state.fieldData;
		outHtml = (
		<div className="panel">
			<div className="panel-heading">
				<h4 className="panel-title draggable">
					<a data-toggle="collapse" href={'#item-' + this.props.ikey}>
						<i className="fa-bars"></i>
						#{this.props.ikey} {fieldData.detail_title}
						<ul className="widget">
							<li><button className="btn-link text-default" title="收合/展開"><i className="fa-chevron-down"></i></button></li>
							<li><button className="btn-link text-danger" title="刪除" onClick={this.deleteItem.bind(this,this.props.ikey)}><i className="fa-times"></i></button></li>
						</ul>
					</a>
				</h4>
			</div>
			<div id={'item-' + this.props.ikey} className="panel-collapse collapse in">
				<div className="panel-body">

					<div className="form-group">
						<label className="col-xs-1 control-label">子單元標題</label>
						<div className="col-xs-2">
							<input type="text" 
							className="form-control"	
							value={fieldData.detail_title}
							onChange={this.changeFDValue.bind(this,'detail_title')}
							maxLength="64"
							required />
						</div>
						<small className="col-xs-1 help-inline">最多60字</small>

						<label className="col-xs-1 control-label">狀態</label>
						<div className="col-xs-2">
							<div className="radio-inline">
								<label>
									<input type="radio" 
									name={'is_open' + this.props.ikey}
									value={false}
									checked={fieldData.i_Hide==false} 
									onChange={this.changeFDValue.bind(this,'i_Hide')}
									/>
									<span>顯示</span>
								</label>
							</div>
							<div className="radio-inline">
								<label>
									<input type="radio" 
									name={'is_open' + this.props.ikey}
									value={true}
									checked={fieldData.i_Hide==true} 
									onChange={this.changeFDValue.bind(this,'i_Hide')}
									/>
									<span>隱藏</span>
								</label>
							</div>
						</div>

						<label className="col-xs-1 control-label">排序</label>
						<div className="col-xs-1">
							<input type="number" 
							className="form-control"	
							value={fieldData.sort}
							onChange={this.changeFDValue.bind(this,'sort')}
							required />
						</div>
						<small className="help-inline col-xs-2">數字大在前面</small>
					</div>
					<div className="form-group">
						<label className="col-xs-1 control-label">內容</label>
						<div className="col-xs-7">
							<input type="text" 
							className="form-control"	
							value={fieldData.detail_content}
							onChange={this.changeFDValue.bind(this,'detail_content')}
							maxLength="255"	/>
						</div>
					</div>
					<div className="form-group">
						{/*<label className="col-xs-1 control-label">圖片上傳</label>
						<div className="col-xs-7">
							<InputFileUpload ikey={this.props.ikey} 
							MainId={fieldData.info_detail_id} 
							FileKind="Photo1" 
							edit_state={fieldData.edit_state}
							ref="reloadFileList"
							refreshFileList={this.props.refreshFileList}
							/>
						</div>*/}
					</div>
				</div>
			</div>
		</div>
		);
		return outHtml;
	}
});

var InputFileUpload = React.createClass({
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
		jqPost(gb_approot+'Sys_Active/ActiveContent/aj_FDelete',{
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
		jqPost(gb_approot+'Sys_Active/ActiveContent/aj_FList',{
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
	createFileUpLoadObject:function(){
			var btn = document.getElementById('upload-btn-' + this.props.ikey);
			var r_this = this;
		  	this.props.uploader  = new ss.SimpleUpload({
		        button: btn,
		        url: gb_approot + 'Sys_Active/ActiveContent/aj_FUpload',
		        data:{
		        	id:this.props.MainId,
		        	fileKind:this.props.FileKind
		        },
		        name: 'fileName',
		        multiple: true,
		        maxSize: 5000,
		        allowedExtensions: ['jpg', 'jpeg', 'png'],
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
		var imgHtml=null;
		if(this.props.MainId!=0){
			imgHtml=(
				<div className="form-control">
				<input type="file" id={'upload-btn-' + this.props.ikey} />
				</div>);
		}else{
			imgHtml=(
				<div className="form-control">
				<small className="col-xs-6 help-inline">請先按儲存後方可上傳圖片</small>
				</div>);
		}
		var outHtml = null;
		outHtml=(				
		<div>
			{imgHtml}
			<p className="help-block">
			{
				this.state.filelist.map(function(itemData,i) {
					var  subOutHtml =
					<span className="img-upload" key={i}>
						<button type="button" className="close" onClick={this.deleteFile.bind(this,itemData.FileName)}>&times;</button>
						<img src={itemData.RepresentFilePath} />
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

