import React from "react";
import { connect } from "react-redux";
import { Alert } from "reactstrap";
import * as Icons from 'react-bootstrap-icons';

export interface ErrorPanelProps {
  isOpen: boolean;
  message: string;
  onClose: () => void;

}

class ErrorPanel extends React.PureComponent<ErrorPanelProps> {
  public render() {
    return (
      <Alert 
        color="danger"
        hidden={!this.props.isOpen}
      >
        <div className="d-flex align-items-baseline justify-content-between">
          <span>{this.props.message}</span>
          <button
            onClick={this.props.onClose}
            className="btn float-right"
          >
            <Icons.X/>
          </button>
        </div>
      </Alert>
    );
  }
};

export default connect()(ErrorPanel);