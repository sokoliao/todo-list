import * as React from 'react';
import * as Icons from 'react-bootstrap-icons';
import { connect } from 'react-redux';
import { RouteComponentProps } from "react-router";
import { Button, Form, FormGroup, Input, Label, Modal, ModalBody, ModalFooter, ModalHeader } from 'reactstrap';
import { TodoTask } from '../model/TodoTask';
import { validators } from '../validation/TodoListValidator';

export interface TaskProps {
  task: TodoTask;
  isNew: boolean;
  onEdit: (id: string) => void;
  onDelete: (id: string) => void;
};

class TaskRecord extends React.PureComponent<TaskProps> {
  public render() {
    return (
      <tr id={this.props.task.id}>
        <td>{this.props.task.name}</td>
        <td>{this.props.task.priority}</td>
        <td>{this.props.task.status}</td>
        <td>
          {' '}
          <button
            onClick={() => this.props.onDelete(this.props.task.id)}
            className="btn float-right"
            disabled={!validators.canDelete(this.props.task)}
          >
            <Icons.TrashFill/>
          </button>
          {' '}
          <button
            onClick={() => this.props.onEdit(this.props.task.id)}
            className="btn float-right"
          >
            <Icons.PencilFill/>
          </button>
        </td>
      </tr>
    );
  }
};

export default connect()(TaskRecord);