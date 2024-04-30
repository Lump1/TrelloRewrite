﻿namespace Trello.Classes
{
    public class TaskValidator
    {
        public static void CheckTaskUpdate(Task taskToUpdate, Task originalTask)
        {
            if (taskToUpdate.Title != null)
            {
                originalTask.Title = taskToUpdate.Title;
            }
            if (taskToUpdate.Iscompleted != null)
            {
                originalTask.Iscompleted = taskToUpdate.Iscompleted;
            }
            if (taskToUpdate.IdCard != null)
            {
                originalTask.IdCard = taskToUpdate.IdCard;
            }
        }
    }
}
