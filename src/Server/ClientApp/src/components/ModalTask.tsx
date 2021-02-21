import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from "react-router";
import { Button, Form, FormGroup, Input, Label, Modal, ModalBody, ModalFooter, ModalHeader } from 'reactstrap';
import { Field } from '../model/Field';
import { TodoTask } from '../model/TodoTask';
import { getTodoStatusNames, parseTodoStatus, TodoTaskStatus } from '../model/TodoTaskStatus';
import { TaskValidationResult } from '../validation/TaskValidationResult';

export interface TaskNameChange {
  field: Field.Name;
  value: string;
}

export interface TaskPriorityChange {
  field: Field.Priority;
  value: number;
}

export interface TaskStatusChange {
  field: Field.Status;
  value: TodoTaskStatus;
}

export type TaskChange = TaskNameChange | TaskPriorityChange | TaskStatusChange

export interface ModalTaskProps {
  title: string;
  onChange: (e: TaskChange) => void;
  isOpen: boolean;
  toggle: () => void;
  model: TodoTask;
  onSubmit: () => void;
  validation: TaskValidationResult;
};

const mapEvent: (event: React.ChangeEvent<HTMLInputElement>) => TaskChange = (event) => {
  switch (event.currentTarget.name) {
    case Field.Name:
      return {
        field: Field.Name,
        value: event.currentTarget.value
      };
    case Field.Priority:
      return {
        field: Field.Priority,
        value: parseInt(event.currentTarget.value)
      };
      break;
    case Field.Status:
      return {
        field: Field.Status,
        value: parseTodoStatus(event.currentTarget.value)
      };
  }
  throw new Error();
}

const mapInputValidityClass: (isPristine: boolean, errors: string[]) => string = (isPristine, errors) => {
  return !isPristine && errors.length !== 0 ? 'is-invalid' : '';
}

class ModalTask extends React.PureComponent<ModalTaskProps> {
  public render() {
    return (
      <Modal isOpen={this.props.isOpen} toggle={this.props.toggle}>
        <ModalHeader toggle={this.props.toggle}>{this.props.title}</ModalHeader>
        <ModalBody>
        <Form onSubmit={this.props.onSubmit}>
          <FormGroup>
            <Label for="newTaskName">Name</Label>
            <Input 
              type="text" 
              name={Field.Name}
              id="newTaskName" 
              placeholder="New task name"
              value={this.props.model.name}
              onChange={e => this.props.onChange(mapEvent(e))}
              className={mapInputValidityClass(
                this.props.validation.isNamePristine,
                this.props.validation.nameErrors)}
            />
            <ul>
              {this.props.validation.nameErrors
                .map(message => <li key={message} className="invalid-feedback">{message}</li>)}
            </ul>
          </FormGroup>
          <FormGroup>
            <Label for="newTaskPriority">Priority</Label>
            <Input 
              type="number"
              name={Field.Priority}
              id="newTaskPriority"
              placeholder="1"
              value={this.props.model.priority}
              onChange={e => this.props.onChange(mapEvent(e))}
              className={mapInputValidityClass(
                this.props.validation.isPriorityPristine,
                this.props.validation.priorityErrors)}
            />
            <ul>
              {this.props.validation.priorityErrors
                .map(message => <li key={message} className="invalid-feedback">{message}</li>)}
            </ul>
          </FormGroup>
          <FormGroup>
            <Label for="newTaskStatus">Status</Label>
            <Input 
              type="select"
              name={Field.Status}
              id="newTaskStatus"
              value={this.props.model.status}
              onChange={e => this.props.onChange(mapEvent(e))}
            >
              { getTodoStatusNames().map(name => <option key={name}>{name}</option>) }
            </Input>
          </FormGroup>
          <ul>
            {this.props.validation.modelErrors.map(message => 
              <li key={message} className="invalid-feedback">{message}</li>)}
          </ul>
        </Form>
        </ModalBody>
        <ModalFooter>
          <Button 
            color="secondary"
            onClick={this.props.onSubmit}
            disabled={
                 this.props.validation.isModelPristine 
              || this.props.validation.modelErrors.length !== 0
              || this.props.validation.nameErrors.length !== 0
              || this.props.validation.priorityErrors.length !== 0}
          >Submit</Button>
        </ModalFooter>
      </Modal>
    );
  }
};

export default connect()(ModalTask);