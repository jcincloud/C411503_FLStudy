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
	categoryCFilter:function(category){
		var valCountry="";
		this.props.category.forEach(function(object, i){
        	if(category==object.val){
  				valCountry=object.Lname;
        	}
    	})
		return valCountry;
	},
	filter:function(value,LName){
		var Label="無";
		CommData[LName].forEach(function(object, i){
        	if(value==object.val){
  				Label=object.label;
        	}
    	})
		return Label;
	},
	render:function(){
		return (

				<tr>
					<td className="text-center"><GridCheckDel iKey={this.props.ikey} chd={this.props.itemData.check_del} delCheck={this.delCheck} /></td>
					<td className="text-center"><GridButtonModify modify={this.modify}/></td>
					<td>{this.props.itemData.title}</td>
					<td>{this.categoryCFilter(this.props.itemData.category)}</td>
					<td>{this.props.itemData.sort}</td>
					<td>{this.props.itemData.i_Hide ? <span className="label label-default">隱藏</span>:<span className="label label-primary">顯示</span>}</td>
					<td>{this.props.itemData.show_index ? <span className="label label-success">顯示</span>:<span className="label label-default">隱藏</span>}</td>
					{/*<td>{this.filter(this.props.itemData.i_Lang,'LangData')}</td>*/}		
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
			searchData:{title:null,category:null},
			edit_type:0,
			checkAll:false,
			category:[]
		};  
	},
	getDefaultProps:function(){
		return{	
			fdName:'fieldData',
			gdName:'searchData',
			apiPathName:gb_approot+'api/HelpfulInfo',
			initPathName:gb_approot+'Active/HelpfulInfo/aj_Init'

		};
	},	
	componentWillMount:function(){
		//在輸出前觸發，只執行一次如果您在這個方法中呼叫 setState() ，會發現雖然 render() 再次被觸發了但它還是只執行一次。
	},
	componentDidMount:function(){
		//只在客戶端執行一次，當渲染完成後立即執行。當生命週期執行到這一步，元件已經俱有 DOM 所以我們可以透過 this.getDOMNode() 來取得 DOM 。
		//如果您想整和其他 Javascript framework ，使用 setTimeout, setInterval, 或者是發動 AJAX 請在這個方法中執行這些動作。
		this.queryGridData(1);
		this.getAjaxInitData();//載入init資料
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
		//設定新增時的編輯器
		if(prevState.edit_type==0 && this.state.edit_type==1){
			CKEDITOR.replace( 'editor1', {});
		}
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
			this.state.fieldData.info_content = CKEDITOR.instances.editor1.getData();//編輯器

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
				ids.push('ids='+this.state.gridData.rows[i].helpful_info_id);
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
		this.setState({edit_type:1,fieldData:{category:6001,i_Lang:'zh-TW'}});
	},
	updateType:function(id){
		jqGet(this.props.apiPathName,{id:id})
		.done(function(data, textStatus, jqXHRdata) {
			this.setState({edit_type:2,fieldData:data.data});
			CKEDITOR.replace( 'editor1', {});
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
	getAjaxInitData:function(){
		jqGet(this.props.initPathName)
		.done(function(data, textStatus, jqXHRdata) {
			this.setState({category:data.options_category});
			//載入下拉是選單內容
		}.bind(this))
		.fail(function( jqXHR, textStatus, errorThrown ) {
			showAjaxError(errorThrown);
		});
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
				<h3 className="title">
					{this.props.caption}
				</h3>
				<form onSubmit={this.handleSearch}>
					<div className="table-responsive">
						<div className="table-header">
							<div className="table-filter">
								<div className="form-inline">
									<div className="form-group">
										{/*<label>語系</label> { }
										<select className="form-control input-sm" 
											value={searchData.i_Lang}
											onChange={this.changeGDValue.bind(this,'i_Lang')}
										>
											<option value="">選擇語系</option>
											{
												CommData.LangData.map(function(itemData,i) {
													return <option key={itemData.val} value={itemData.val}>{itemData.label}</option>;
												})
											}
										</select> { }*/}
											<label>分類</label> { }
											<select className="form-control input-sm" 
											value={searchData.category}
											onChange={this.changeGDValue.bind(this,'category')}>
											<option value="">全部</option>
										{
											this.state.category.map(function(itemData,i) {
												return <option key={i} value={itemData.val}>{itemData.Lname}</option>
											})
										}
											</select>
										</div> { }
									<div className="form-group">
										<label>標題</label> { }
										<input type="text" className="form-control input-sm" 
										value={searchData.title}
										onChange={this.changeGDValue.bind(this,'title')}
										placeholder="請輸入關鍵字..." /> { }
										<button className="btn-primary btn-sm" type="submit"><i className="fa-search"></i> 搜尋</button>
									</div>
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
									<th className="col-xs-4">標題</th>
									<th className="col-xs-2">分類</th>
									<th className="col-xs-1">排序</th>
									<th className="col-xs-1">狀態</th>
									<th className="col-xs-1">顯示於首頁</th>
									{/*<th className="col-xs-1">語系</th>*/}
								</tr>
							</thead>
							<tbody>
								{
								this.state.gridData.rows.map(function(itemData,i) {
								return <GridRow 
								key={i}
								ikey={i}
								primKey={itemData.helpful_info_id} 
								itemData={itemData} 
								delCheck={this.delCheck}
								updateType={this.updateType}
								category={this.state.category}							
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
					<li><i className="fa-list-alt"></i> {this.props.menuName}</li>
				</ul>
				<h4 className="title">{this.props.caption} 資料維護</h4>
				<div className="alert alert-warning"><p><strong className="text-danger">紅色標題</strong> 為必填項目。</p></div>
				<form className="form-horizontal" onSubmit={this.handleSubmit}>
					<div className="form-group">
						{/*<label className="col-xs-1 control-label text-danger">語系</label>
						<div className="col-xs-3">
							<select className="form-control" 
								value={fieldData.i_Lang}
								onChange={this.changeFDValue.bind(this,'i_Lang')}
								required>
									{
										CommData.LangData.map(function(itemData,i) {
											return <option key={itemData.val} value={itemData.val}>{itemData.label}</option>;
										})
									}
							</select>						
						</div>*/}
						<label className="col-xs-1 control-label">分類</label>
						<div className="col-xs-3">
							<select className="form-control" value={fieldData.category} 
							onChange={this.changeFDValue.bind(this,'category')}
							required>
							{
								this.state.category.map(function(itemData,i) {
								return <option key={i} value={itemData.val}>{itemData.Lname}</option>
								})
							}
							</select>
						</div>
					</div>
					<div className="form-group">
						<label className="col-xs-1 control-label">代表圖</label>
						<div className="col-xs-7">
							<MasterImageUpload 
							FileKind="Photo1" 
							MainId={fieldData.helpful_info_id}
							ParentEditType={this.state.edit_type}
							url_upload={gb_approot + 'Active/HelpfulInfo/axFUpload'}
							url_list={gb_approot+'Active/HelpfulInfo/axFList'}
							url_delete={gb_approot+'Active/HelpfulInfo/axFDelete'}
							url_sort={gb_approot+'Active/HelpfulInfo/axFSort'}
							/>
						</div>
						<small className="help-inline col-xs-4 text-danger">限 1 張圖片</small>
					</div>

					{/*<div className="form-group">
						<label className="col-xs-1 control-label">內頁圖片</label>
						<div className="col-xs-6">
							<MasterImageUpload 
							FileKind="Photo2" 
							MainId={fieldData.helpful_info_id}
							ParentEditType={this.state.edit_type}
							url_upload={gb_approot + 'Active/HelpfulInfo/axFUpload'}
							url_list={gb_approot+'Active/HelpfulInfo/axFList'}
							url_delete={gb_approot+'Active/HelpfulInfo/axFDelete'}
							url_sort={gb_approot+'Active/HelpfulInfo/axFSort'}
							/>
						</div>
						<small className="help-inline col-xs-5 text-danger">限上傳 1 張圖片</small>
					</div>*/}

					<div className="form-group">
						<label className="col-xs-1 control-label text-danger">標題</label>
						<div className="col-xs-7">
							<input type="text" 
							className="form-control"	
							value={fieldData.title}
							onChange={this.changeFDValue.bind(this,'title')}
							maxLength="64"
							required />						
						</div>
						<small className="col-xs-4 help-inline">最多64字</small>
					</div>

					<div className="form-group">
						<label className="col-xs-1 control-label">狀態</label>
						<div className="col-xs-2">
							<div className="radio-inline">
								<label>
									<input type="radio" 
											name="i_Hide"
											value={true}
											checked={fieldData.i_Hide===true} 
											onChange={this.changeFDValue.bind(this,'i_Hide')}
									/>
									<span>隱藏</span>
								</label>
							</div>
							<div className="radio-inline">
								<label>
									<input type="radio" 
											name="i_Hide"
											value={false}
											checked={fieldData.i_Hide===false} 
											onChange={this.changeFDValue.bind(this,'i_Hide')}
											/>
									<span>顯示</span>
								</label>
							</div>
						</div>
						<label className="col-xs-1 control-label">顯示於首頁</label>
						<div className="col-xs-2">
							<div className="radio-inline">
								<label>
									<input type="radio" 
											name="show_index"
											value={false}
											checked={fieldData.show_index===false} 
											onChange={this.changeFDValue.bind(this,'show_index')}
									/>
									<span>隱藏</span>
								</label>
							</div>
							<div className="radio-inline">
								<label>
									<input type="radio" 
											name="show_index"
											value={true}
											checked={fieldData.show_index===true} 
											onChange={this.changeFDValue.bind(this,'show_index')}
											/>
									<span>顯示</span>
								</label>
							</div>
						</div>
						<label className="col-xs-1 control-label text-danger">排序</label>
						<div className="col-xs-1">
							<input type="number" 
									className="form-control"	
									value={fieldData.sort}
									onChange={this.changeFDValue.bind(this,'sort')}
									maxLength="64"
									required />						
						</div>
						<small className="col-xs-2 help-inline">數字越大越前面</small>
					</div>			

					<div className="form-group">
						<label className="col-xs-1 control-label">簡介</label>
							<div className="col-xs-10">
								<textarea type="date" className="form-control" rows="3"
									value={fieldData.list_intro}
									onChange={this.changeFDValue.bind(this,'list_intro')}
									maxLength="400"/>
							</div>
					</div>

					<div className="form-group">
						<label className="col-xs-1 control-label">內容</label>
							<div className="col-xs-10">
								<textarea type="date" className="form-control" rows="3" id="editor1"
									value={fieldData.info_content}
									onChange={this.changeFDValue.bind(this,'info_content')} />
							</div>
					</div>

					<div className="form-action">
						<div className="col-xs-4 col-xs-offset-1">
							<button type="submit" className="btn-primary"><i className="fa-check"></i> 儲存</button>
							<button className="col-xs-offset-1" type="button" onClick={this.noneType}><i className="fa-times"></i> 回前頁</button>
						</div>
					</div>
				</form>
			</div>
				);
		}else{
			outHtml=(<span>No Page</span>);
		}

		return outHtml;
	}
});