﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace YouTrack.Rest.Features.General.Issues
{
    [Binding]
    [Scope(Feature = "Get Attachments of an Issue")]
    public class GetAttachmentsOfAnIssueSteps : IssueSteps
    {
        [Given(@"I have created an issue")]
        public void GivenIHaveCreatedAnIssue()
        {
            SaveIssue(StepHelper.CreateIssue("SB", "Testing attachments", "I will have two of them"));
        }

        [Given(@"attached two files to the issue")]
        public void GivenAttachedTwoFilesToTheIssue()
        {
            StepHelper.AttachFile(GetSavedIssue(), @"Steps\Attachments\I-don't-usually-test-my-code-But-when-I-do-it,-I-do-it-in-production.jpg");
            StepHelper.AttachFile(GetSavedIssue(), @"Steps\Attachments\I-don't-usually-test-my-code-But-when-I-do-it,-I-do-it-in-production.jpg");
        }

        [When(@"I get the attachments of the issue")]
        public void WhenIGetTheAttachmentsOfTheIssue()
        {
            IIssue issue = GetSavedIssue();
            IEnumerable<IAttachment> attachments = StepHelper.GetAttachments(GetSavedIssue());

            ScenarioContext.Current.Set(attachments);
        }

        [Then(@"I get two attached files")]
        public void ThenIGetTwoAttachedFiles()
        {
            IEnumerable<IAttachment> attachments = ScenarioContext.Current.Get<IEnumerable<IAttachment>>();

            Assert.That(attachments.Count(), Is.EqualTo(2));
        }
    }
}
