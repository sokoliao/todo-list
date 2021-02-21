import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from "react-router";
import { Button, Form, FormGroup, Input, Label, Modal, ModalBody, ModalFooter, ModalHeader } from 'reactstrap';
import { TodoTask } from '../model/TodoTask';

export interface DeleteTaskDialogProps {
  isOpen: boolean;
  toggle: () => void;
  model: TodoTask;
  onSubmit: () => void;
};


class DeleteTaskDialog extends React.PureComponent<DeleteTaskDialogProps> {
  public render() {
    return (
      <Modal isOpen={this.props.isOpen} toggle={this.props.toggle}>
        <ModalHeader toggle={this.props.toggle}>Delete task</ModalHeader>
        <ModalBody>
          {`Are you sure you want to remove task "${this.props.model.name}"`}
        </ModalBody>
        <ModalFooter>
          <Button color="danger" onClick={this.props.onSubmit}>Delete</Button>
        </ModalFooter>
      </Modal>
    );
  }
};

export default connect()(DeleteTaskDialog);