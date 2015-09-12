//帳號輸入
var InputAccount = React.createClass({
	mixins: [React.addons.LinkedStateMixin], 
	getInitialState: function() {  
		return { 
		};  
	},

	getValue:function(){
		return this.state.value;
	},
	render:function(){
		return (
			<div className="form-group has-feedback">
				<label className="control-label">{this.props.label} Username</label>
				<input 
				className="form-control"
				name={this.props.idName}
				id={this.props.idName}
				type="text"
				valueLink={this.linkState('value')}
				tabIndex="1"
				placeholder={this.props.memo}
				required />
				<i className="fa-user form-control-feedback"></i>
			</div>
			);
		}
});

//密碼輸入
var InputPassword = React.createClass({
	mixins: [React.addons.LinkedStateMixin], 
	getInitialState: function() {  
		return { 
		};  
	},
	getValue:function(){
		return this.state.value;
	},
	clearValue:function(){
		this.setState({value:null});
	},
	render:function(){
		return (
			<div className="form-group has-feedback">
				<label className="control-label">{this.props.label} Username</label>
				<input 
				className="form-control"
				name={this.props.idName}
				id={this.props.idName}
				type="password"
				valueLink={this.linkState('value')}
				tabIndex="2"
				placeholder={this.props.memo}
				required />
				<i className="fa-lock form-control-feedback"></i>
			</div>
			);
	}
});

//驗證碼入
var Inputvalidate = React.createClass({
	mixins: [React.addons.LinkedStateMixin], 
	getInitialState: function() {  
		return { 
			validateUrl: this.getValidateUrl()
		};  
	},
	getValue:function(){
		return this.state.value;
	},
	onChange:function(event){
		this.setState({value:event.target.value.toUpperCase()});
	},
	getValidateUrl:function(){
		return this.props.webRoot + '_Code/Ashx/ValidateCode.ashx?vn=CheckCode&t=' + uniqid();
	},
	reLoadValidateUrl:function(){
		this.setState({validateUrl: this.getValidateUrl(),value:null});
	},
	render:function(){
		return (
                    <div className="form-group">
                        <label className="control-label">{this.props.label} Code</label>
                        <div className="row">
                            <div className="col-xs-3">
                                <img alt={this.props.label} src={this.state.validateUrl} />
                            </div>
                            <div className="col-xs-5">
                                <input type="text"
                                       className="form-control"
                                       tabIndex="3"
                                       onChange={this.onChange}
                                       value={this.state.value}
                                       required
                                       placeholder={this.props.memo} />
                            </div>
                            <div className="col-xs-4">
                                <button 
                                className="btn btn-warning" 
                                type="button" 
                                tabIndex="-1"
                                onClick={this.reLoadValidateUrl}
                                ><i className="fa-refresh"></i> 重取</button>
                            </div>
                        </div>
                    </div>
			);
	}
});

//語系登入
var Inputlang = React.createClass({
	mixins: [React.addons.LinkedStateMixin], 
	getInitialState: function() {  
		return { 
			value:'zh-TW'
		};  
	},
	getValue:function(){
		return this.state.value;
	},
	render:function(){
		return (
			<div className="form-group has-feedback">
				<label className="control-label">{this.props.label} Language</label>
				<select 
				className="form-control"
				tabindex="1"
				name={this.props.idName}
				id={this.props.idName}
				valueLink={this.linkState('value')}
				placeholder={this.props.memo}
				required >
				{
					CommData.LangData.map(function(itemData,i) {
						return <option key={itemData.val} value={itemData.val}>{itemData.label}</option>;
					})
				}
				</select>
			</div>
			);
		}
});

//表單登錄
var LoginForm = React.createClass({
	mixins: [React.addons.LinkedStateMixin], 
	getInitialState: function() {  
		return {
		};  
	},
	getDefaultProps:function(){
		return{};
	},	
	componentWillMount:function(){
		//在輸出前觸發，只執行一次如果您在這個方法中呼叫 setState() ，會發現雖然 render() 再次被觸發了但它還是只執行一次。
	},
	componentDidMount:function(){
		//只在客戶端執行一次，當渲染完成後立即執行。當生命週期執行到這一步，元件已經俱有 DOM 所以我們可以透過 this.getDOMNode() 來取得 DOM 。
		//如果您想整和其他 Javascript framework ，使用 setTimeout, setInterval, 或者是發動 AJAX 請在這個方法中執行這些動作。
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
	handleChange: function (newValue) {  
		//this.setState({text: newValue});
	}, 
	handleSubmit: function(e) {
		e.preventDefault();
		//console.log(this.refs.account.getValue(),this.refs.password.getValue(),this.refs.validate.getValue());
		var data = {
			account:this.refs.account.getValue(),
			password:this.refs.password.getValue(),
			validate:this.refs.validate.getValue(),
			//lang:this.refs.lang.getValue(),
			lang:'zh-TW',
			rememberme:false
		};

		var jqxhr = $.ajax({
							type: "POST",
							url: ajax_Login,
							data: data,
							dataType: 'json'
							})
					.done(function(data, textStatus, jqXHRdata) {
						if(data.result){
							document.location.href=data.url;
						}else{
							this.refs.validate.reLoadValidateUrl();
							this.refs.password.clearValue();
							alert(data.message);
						}
					 }.bind(this))
					.fail(function( jqXHR, textStatus, errorThrown ) {
						showAjaxError(errorThrown);
					});
		return;
	},
	render: function() {
		return (
			<div>
				<h3>{this.props.systemName}</h3>
				<form onSubmit={this.handleSubmit}>
					<InputAccount label={'帳號'} idName={'account'} memo={'帳號'} ref="account" />
					<InputPassword label={'密碼'} idName={'password'} memo={'密碼'} ref="password" />
					{/*<Inputlang label={'語系'} idName={'lang'} memo={'語系'} ref="lang" />*/}
					<Inputvalidate label={'驗證碼'} idName={'validate'} memo={'驗證碼'} ref="validate" webRoot={this.props.webRoot} />
                    <div className="form-action">
                        <div className="row">
                            <div className="col-xs-4">
                                <div className="checkbox">
                                    <label>
                                        <input type="checkbox" tabIndex="-1" />
                                        <span>記住</span>
                                    </label>
                                </div>
                            </div>
                            <div className="col-xs-3 col-xs-offset-1">
                                <button className="btn btn-info" tabIndex="4" type="submit"><i className="fa-key"></i> 登錄</button>
                            </div>
                            <div className="col-xs-4">
                                <button tabIndex="-1" type="button"><i className="fa-question-circle"></i> 忘記密碼</button>
                            </div>
                        </div>
                    </div>
				</form>
			</div>
			);
	}
});