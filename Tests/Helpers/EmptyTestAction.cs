﻿using DataLayer.DataClasses.Concrete;
using GenericServices;
using GenericServices.Actions;
using GenericServices.Core;

namespace Tests.Helpers
{
    public interface IEmptyTestAction : IActionDefn<int, Tag>
    {
    }

    public class EmptyTestAction : ActionBase, IEmptyTestAction
    {

        /// <summary>
        /// If true then the caller should call EF SubmitChanges if the method exited with status IsValid and
        /// it looks to see if the data part has a ICheckIfWarnings and if the WriteEvenIfWarning is false
        /// and there are warnings then it does not call SubmitChanges
        /// </summary>
        public override bool SubmitChangesOnSuccess { get { return false; } }

        /// <summary>
        /// This allows the action to configure what it supports, which then affects what the user sees
        /// Note: it must be a constant as it is read just after the action is created
        /// </summary>
        public override ActionFlags ActionConfig
        {
            get { return ActionFlags.NoProgressSent | ActionFlags.NoMessagesSent | ActionFlags.CancelNotSupported; }
        }

        //-------------------------------------------

        public ISuccessOrErrors<int> DoAction(IActionComms actionComms, Tag actionData)
        {
            ISuccessOrErrors<int> status = new SuccessOrErrors<int>();

            //we use the TagId for testing
            //0 means success
            //1 means success, but with warning
            //2 and above mean fail

            if (actionData.TagId == 1)
                status.AddWarning("This is a warning message");

            return actionData.TagId <= 1
                ? status.SetSuccessWithResult(actionData.TagId, "Successful")
                : status.AddSingleError("forced fail");
        }
    }
}