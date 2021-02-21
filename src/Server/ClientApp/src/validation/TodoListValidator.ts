import { TodoTask } from "../model/TodoTask";
import { TodoTaskStatus } from "../model/TodoTaskStatus";
import { TaskValidationErrors } from "./TaskValidationResult";

const taskValidator: (newTask: TodoTask) => TaskValidationErrors = (newTask) => {
  const errors: TaskValidationErrors = {
    nameErrors: [],
    priorityErrors: [],
    modelErrors: []
  }
  if (newTask.name.trim() === '') {
    errors.nameErrors.push('non-empty value is expected');
  }
  if (!Number.isInteger(newTask.priority)) {
    errors.priorityErrors.push('integer value is expected');
  }
  if (newTask.priority <= 0) {
    errors.priorityErrors.push('positive, non-zero value is expected');
  }
  return errors;
}

export interface TodoListValidators {
  onCreate: (task: TodoTask, tasks: TodoTask[]) => TaskValidationErrors;
  onEdit: (task: TodoTask, tasks: TodoTask[]) => TaskValidationErrors;
  canDelete: (task: TodoTask) => boolean;
}

export const validators : TodoListValidators = {
  onCreate: (task, tasks) => {
    const errors: TaskValidationErrors = taskValidator(task);
    if (tasks.find(t => t.name === task.name)) {
      errors.nameErrors.push('unique value is expected');
    }
    return errors;
  },
  onEdit: (task, tasks) => {
    const errors: TaskValidationErrors = taskValidator(task);
    if (tasks.find(t => t.name === task.name  && t.id !== task.id)) {
      errors.nameErrors.push('unique value is expected');
    }
    return errors;
  },
  canDelete: (taskToDelete: TodoTask): boolean => {
    return taskToDelete.status === TodoTaskStatus.Completed;
  }
}