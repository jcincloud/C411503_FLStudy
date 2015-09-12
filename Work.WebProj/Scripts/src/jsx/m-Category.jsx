var GridRow = React.createClass({
	mixins: [React.addons.LinkedStateMixin], 
	getInitialState: function() {  
		return { 			
		};  
	},
	delCheck:function(i,chd){
		this.props.delCheck(i,chd);
	},
	subCheck:function(i,chd){
		this.props.subCheck(i,chd);
	},
	render:function(){
		var subHtml=null;
		if(this.props.itemData.check_sub){
			subHtml=<GridSubForm ref="GridSubForm" MainId={this.props.primKey} i_Lang={this.props.i_Lang}/>;
		}
		return (
			<tbody>
				<tr>
					{/*<td className="text-center"><GridCheckDel iKey={this.props.ikey} chd={this.props.itemData.check_del} delCheck={this.delCheck} /></td>*/}
					<td className="text-center"><GridButtonSub iKey={this.props.ikey} chd={this.props.itemData.check_sub}  subCheck={this.subCheck}/></td>
					<td>{this.props.itemData.l1_name}</td>
					<td>{this.props.itemData.memo}</td>
				</tr>
				{subHtml}{/*裡面放 子表單(GridSubForm)點展開才顯示--(底層)-->子表單內容(GridSubRow)*/}
			</tbody>
			);
		}
});
//子表單內容
var GridSubRow = React.createClass({
	mixins: [React.addons.LinkedStateMixin], 
	getInitialState: function() {  
		return { 
		};  
	},
	delCheck:function(i,chd){
		this.props.delCheck(i,chd);
	},
	modify:function(){
		console.log(this.props.primKey);
		this.props.updateType(this.props.primKey);
	},
	render:function(){
		return (

				<tr>
					<td className="text-center"><i className="fa-bars text-muted draggable"></i></td>
					<td className="text-center"><GridCheckDel iKey={this.props.ikey} chd={this.props.itemData.check_del} delCheck={this.delCheck} /></td>
					<td className="text-center"><GridButtonPopupWindow modify={this.modify} MainId={this.props.MainId}/></td>
					<td>{this.props.itemData.l2_name}</td>
					<td>{this.props.itemData.sort}</td>
					<td>{this.props.itemData.i_Hide?<span className="label label-default">隱藏</span>:<span className="label label-primary">顯示</span>}</td>
				</tr>
			);
		}
});
//子表單
var GridSubForm = React.createClass({
	mixins: [React.addons.LinkedStateMixin,SortableMixin], 
	getInitialState: function() {  
		return {
			gridSubData:{rows:[],page:1},
			fieldData:{},
			searchData:{l1_id:this.props.MainId,i_Lang:this.props.i_Lang},
			edit_type:0,
			checkAll:false,
			refreshFileList:false,
			gridSubTbody:[],
			updateSortVal:[]
		};  
	},
	getDefaultProps:function(){
		return{	
			fdName:'fieldData',
			gdName:'searchData',
			apiSubPathName:gb_approot+'api/AllCategoryL2',
			apiUpdateSortPath:gb_approot+'Active/Category/UpdateSort'
		};
	},
	componentDidMount:function(){
		this.queryGridData(1);
	},
	gridData:function(page){

		var parms = {
			page:0
		};

		if(page==0){
			parms.page=this.state.gridSubData.page;
		}else{
			parms.page=page;
		}

		$.extend(parms, this.state.searchData);

		return jqGet(this.props.apiSubPathName,parms);
	},
	queryGridData:function(page){
		this.gridData(page)
		.done(function(data, textStatus, jqXHRdata) {
			this.setState({gridSubData:data,gridSubTbody:data.rows});
		}.bind(this))
		.fail(function(jqXHR, textStatus, errorThrown) {
			showAjaxError(errorThrown);
		});
	},
	checkAll:function(){

		var newState = this.state;
		newState.checkAll = !newState.checkAll;
		for (var prop in this.state.gridSubData.rows) {
			this.state.gridSubTbody[prop].check_del=newState.checkAll;
		}
		this.setState(newState);
	},
	delCheck:function(i,chd){

		var newState = this.state;
		this.state.gridSubTbody[i].check_del = !chd;
		this.setState(newState);
	},
	checkData:function(){
		if(this.state.fieldData.l2_name=="" || this.state.fieldData.l2_name==null){
			return false;
		}else{
			return true;
		}
	},
	handleSubmit: function(e) {
		e.preventDefault();
		if(!this.checkData){
			alert("資料填寫不完整~!");
		}
		if(this.state.edit_type==1){
			jqPost(this.props.apiSubPathName,this.state.fieldData)
			.done(function(data, textStatus, jqXHRdata) {
				if(data.result){
					tosMessage(null,'新增完成',1);
					this.updateType(data.id);
					this.queryGridData(1);//重整子選單資料
				}else{
					alert(data.message);
				}
			}.bind(this))
			.fail(function( jqXHR, textStatus, errorThrown ) {
				showAjaxError(errorThrown);
			});
		}		
		else if(this.state.edit_type==2){
			jqPut(this.props.apiSubPathName,this.state.fieldData)
			.done(function(data, textStatus, jqXHRdata) {
				if(data.result){
					tosMessage(null,'修改完成',1);
					this.queryGridData(1);//重整子選單資料
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
		for(var i in this.state.gridSubTbody){
			if(this.state.gridSubTbody[i].check_del){
				ids.push('ids='+this.state.gridSubTbody[i].all_category_l2_id);
			}
		}

		if(ids.length==0){
			tosMessage(null,'未選擇刪除項',2);
			return;
		}

		jqDelete(this.props.apiSubPathName + '?' + ids.join('&'),{})			
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
	insertType:function(){
		this.setState({edit_type:1,fieldData:{all_category_l1_id:this.props.MainId}});
	},
	updateType:function(id){
		jqGet(this.props.apiSubPathName,{id:id})
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
			this.setState({edit_type:0,gridData:data,});
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
	updateSort:function(){
		jqPost(this.props.apiUpdateSortPath,{Data:this.state.updateSortVal})
		.done(function(data, textStatus, jqXHRdata) {
			//console.log(data.result);
		}.bind(this))
		.fail(function( jqXHR, textStatus, errorThrown ) {
			showAjaxError(errorThrown);
		});
	},
	handleSort: function (evt) { 
		var newState = this.state;
		var n = newState.gridSubTbody.length;
		var items=[];
		for (var i in newState.gridSubTbody) {
			newState.gridSubTbody[i].sort=n;
			//更新排序所用的object(item)和array(items)
			var item={id:newState.gridSubTbody[i].all_category_l2_id,sort:n};
			items.push(item);
			//
			n--;
		}
		newState.updateSortVal=items;
		newState.refreshFileList = true;
		this.setState(newState);
		this.setState({refreshFileList:false});
		this.updateSort();
	},
	sortableOptions: {
        ref: "SortTbody",
        model:'gridSubTbody',
        group: "shared"
    },
	render:function(){
		var fieldData = this.state.fieldData;

		return (
                <tr className="sub-grid">
                    <td className="fold">
                        <div className="row">
                            <div className="col-xs-6 col-xs-offset-6 text-center">
                                <i className="fa-chevron-right"></i>
                            </div>
                        </div>
                    </td>
                    <td colSpan="3">
                        <div className="row">
                            <div className="col-xs-10">
                                <table>
                                    <thead>
                                        <tr>
                                        	<th className="col-xs-1"></th>
											<th className="col-xs-1 text-center">
												<label className="cbox">
													<input type="checkbox" checked={this.state.checkAll} onChange={this.checkAll} />
													<i className="fa-check"></i>
												</label>
											</th> 
	                                        <th className="col-xs-1 text-center">修改</th>
                                            <th className="col-xs-5">項目</th>
                                            <th className="col-xs-2">排序</th>
                                            <th className="col-xs-2">狀態</th>
                                        </tr>
                                    </thead>
                                    <tbody ref="SortTbody">
								{
								this.state.gridSubTbody.map(function(itemData,i) {
								return <GridSubRow 
								key={itemData.all_category_l2_id}
								ikey={i}
								MainId={this.props.MainId}
								primKey={itemData.all_category_l2_id} 
								itemData={itemData} 
								delCheck={this.delCheck}
								updateType={this.updateType}
								refreshFileList={this.state.refreshFileList}						
								/>;
								}.bind(this))
								}
                                    </tbody>
                                </table>
                    			<GridNavPageUsePopup
                    				MainId={this.props.MainId}
									StartCount={this.state.gridSubData.startcount}
									EndCount={this.state.gridSubData.endcount}
									RecordCount={this.state.gridSubData.records}
									TotalPage={this.state.gridSubData.total}
									NowPage={this.state.gridSubData.page}
									onQueryGridData={this.queryGridData}
									InsertType={this.insertType}
									deleteSubmit={this.deleteSubmit}
								/>
								


				<div id={'myModal-'+this.props.MainId} className="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
    				<div className="modal-dialog">
      					<div className="modal-content">
        					<div className="modal-header">
          						<button type="button" className="close" data-dismiss="modal" aria-hidden="true">&times;</button>
          						<h4 className="modal-title" id="myModalLabel">分類項目資料維護</h4>
        					</div>
        					<form className="form-horizontal" onSubmit={this.handleSubmit}>
        						<div className="modal-body">
        							<div className="alert alert-warning"><p>以下皆為 <strong className="text-danger">必填項目</strong> 。</p></div>
					          		<div className="form-group">
										<label className="col-xs-2 control-label">項目名稱</label>
										<div className="col-xs-6">
											<input type="text" 
												className="form-control"	
												value={fieldData.l2_name}
												onChange={this.changeFDValue.bind(this,'l2_name')}
												maxLength="64"
												required />						
										</div>
										<small className="col-xs-4 help-inline">最多64字</small>
									</div>
          							<div className="form-group">
										<label className="col-xs-2 control-label">排序</label>
										<div className="col-xs-6">
											<input type="number" 
												className="form-control"	
												value={fieldData.sort}
												onChange={this.changeFDValue.bind(this,'sort')}
												maxLength="64"
												required />						
										</div>
										<small className="col-xs-4 help-inline">數字越大越前面</small>
									</div>
									
									<div className="form-group">
										<label className="col-xs-2 control-label">狀態</label>
										<div className="col-xs-3">
											<div className="radio-inline">
												<label>
													<input 	type="radio" 
															name="i_Hide"
															value={true}
															checked={fieldData.i_Hide===true} 
															onChange={this.changeFDValue.bind(this,'i_Hide')} />
													<span>隱藏</span>
												</label>
											</div>
											<div className="radio-inline">
												<label>
													<input type="radio" 
															name="i_Hide"
															value={false}
															checked={fieldData.i_Hide===false} 
															onChange={this.changeFDValue.bind(this,'i_Hide')}/>
													<span>顯示</span>
												</label>
											</div>
										</div>				
									</div>

        						</div>
	        					<div className="modal-footer">
		        					<div className="col-xs-4 col-xs-offset-2">
		        						<button type="button" className="btn-primary" onClick={this.handleSubmit}><i className="fa-check"></i> 儲存</button>
		        						<button className="col-xs-offset-1" type="button" onClick={this.noneType} data-dismiss="modal"><i className="fa-times"></i>關閉</button>
		        					</div>
	        					</div>
	        				</form>
      					</div>
   					</div>
				</div>


                            </div>
                        </div>
                    </td>
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
			searchData:{i_Lang:'zh-TW'},
			edit_type:0,
			checkAll:false
		};  
	},
	getDefaultProps:function(){
		return{	
			fdName:'fieldData',
			gdName:'searchData',
			apiPathName:gb_approot+'api/AllCategoryL1',
			apiSubPathName:gb_approot+'api/AllCategoryL2'

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
	handleSubmit: function(e) {
		e.preventDefault();
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
			this.state.fieldData.news_content = CKEDITOR.instances.editor1.getData();//編輯器

			jqPut(this.props.apiPathName,this.state.fieldData)
			.done(function(data, textStatus, jqXHRdata) {
				if(data.result){
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
				ids.push('ids='+this.state.gridData.rows[i].news_id);
			}
		}

		if(ids.length==0){
			tosMessage(null,'未選擇刪除項',2);
			return;
		}

		jqDelete(this.props.apiPathName + '?' + ids.join('&'),{})			
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
	subCheck:function(i,chd){

		var newState = this.state;
		this.state.gridData.rows[i].check_sub = !chd;
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
	changeLangVal:function(e){
		this.setState({searchData:{i_Lang:e.target.value}});
		//切換語系時把所以有展開的項目收合,以免程式錯亂
		this.state.gridData.rows.forEach(function(obj,i){
			obj.check_sub=false;
		})
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
				<div className="alert alert-warning clear" role="alert">
					<p>點選 <i className="fa-bars"></i> 並拖曳，可修改排列順序。</p>
				</div>
				<form onSubmit={this.handleSearch}>
					<div className="table-responsive">
						<table>
							<thead>
								<tr>
									<th className="col-xs-1 text-center">展開/收合</th>
									<th className="col-xs-5">項目</th>
									<th className="col-xs-6">說明</th>
								</tr>
							</thead>
								{
								this.state.gridData.rows.map(function(itemData,i) {
								return <GridRow
								ref="GridRow"
								i_Lang={this.state.searchData.i_Lang}
								key={itemData.all_category_l1_id}
								ikey={i}
								primKey={itemData.all_category_l1_id} 
								itemData={itemData} 
								delCheck={this.delCheck}
								subCheck={this.subCheck}
								updateType={this.updateType}						
								/>;
								}.bind(this))
								}
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
		else
		{
			outHtml=(<span>No Page</span>);
		}

		return outHtml;
	}
});
